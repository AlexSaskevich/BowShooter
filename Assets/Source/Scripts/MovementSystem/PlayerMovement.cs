using System;
using CMF;
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    public class PlayerMovement
    {
        private readonly InputReader _inputReader;
        private readonly CeilingDetector _ceilingDetector;
        private readonly Transform _cameraTransform;
        private readonly Transform _playerTransform;
        private readonly Mover _mover;

        private bool _jumpInputIsLocked;
        private bool _jumpKeyWasPressed;
        private bool _jumpKeyWasLetGo;
        private bool _jumpKeyIsPressed;
        private Vector3 _momentum = Vector3.zero;
        private float _currentJumpStartTime;
        private Vector3 _savedVelocity = Vector3.zero;
        private Vector3 _savedMovementVelocity = Vector3.zero;
        private MovementState _currentMovementState = MovementState.Falling;

        public PlayerMovement(InputReader inputReader, Mover mover, Transform playerTransform,
            PlayerMovementParameters playerMovementParameters, Transform cameraControls,
            CeilingDetector ceilingDetector = null)
        {
            _inputReader = inputReader;
            _mover = mover;
            _playerTransform = playerTransform;
            PlayerMovementParameters = playerMovementParameters;
            _cameraTransform = cameraControls;
            _ceilingDetector = ceilingDetector;
        }

        public event Action<Vector3> Jumped;
        public event Action<Vector3> Landed;

        public PlayerMovementParameters PlayerMovementParameters { get; private set; }

        public void HandleJumpKeyInput()
        {
            bool newJumpKeyPressedState = IsJumpKeyPressed();

            if (_jumpKeyIsPressed == false && newJumpKeyPressedState)
            {
                _jumpKeyWasPressed = true;
            }

            if (_jumpKeyIsPressed && newJumpKeyPressedState == false)
            {
                _jumpKeyWasLetGo = true;
                _jumpInputIsLocked = false;
            }

            _jumpKeyIsPressed = newJumpKeyPressedState;
        }

        public void ControllerUpdate()
        {
            _mover.CheckForGround();

            _currentMovementState = DetermineControllerState();

            HandleMomentum();

            HandleJumping();

            Vector3 velocity = Vector3.zero;

            if (_currentMovementState == MovementState.Grounded)
            {
                velocity = CalculateMovementVelocity();
            }

            Vector3 worldMomentum = _momentum;

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                worldMomentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            velocity += worldMomentum;

            _mover.SetExtendSensorRange(IsGrounded());

            _mover.SetVelocity(velocity);

            _savedVelocity = velocity;

            _savedMovementVelocity = CalculateMovementVelocity();

            _jumpKeyWasLetGo = false;
            _jumpKeyWasPressed = false;

            if (_ceilingDetector != null)
            {
                _ceilingDetector.ResetFlags();
            }
        }

        private Vector3 CalculateMovementDirection()
        {
            Vector3 velocity = Vector3.zero;

            if (_cameraTransform == null)
            {
                velocity += _playerTransform.right * _inputReader.MoveInput.x;
                velocity += _playerTransform.forward * _inputReader.MoveInput.y;
            }
            else
            {
                velocity += Vector3.ProjectOnPlane(_cameraTransform.right, _playerTransform.up).normalized *
                            _inputReader.MoveInput.x;
                velocity += Vector3.ProjectOnPlane(_cameraTransform.forward, _playerTransform.up).normalized *
                            _inputReader.MoveInput.y;
            }

            if (velocity.magnitude > 1f)
            {
                velocity.Normalize();
            }

            return velocity;
        }

        private Vector3 CalculateMovementVelocity()
        {
            Vector3 velocity = CalculateMovementDirection();

            velocity *= PlayerMovementParameters.MovementSpeed;

            return velocity;
        }

        private bool IsJumpKeyPressed()
        {
            return _inputReader.IsJumpButtonPressed;
        }

        private MovementState DetermineControllerState()
        {
            bool isRising = IsRisingOrFalling() && VectorMath.GetDotProduct(GetMomentum(), _playerTransform.up) > 0f;
            bool isSliding = _mover.IsGrounded() && IsGroundTooSteep();

            if (_currentMovementState == MovementState.Grounded)
            {
                if (isRising)
                {
                    OnGroundContactLost();
                    return MovementState.Rising;
                }

                if (!_mover.IsGrounded())
                {
                    OnGroundContactLost();
                    return MovementState.Falling;
                }

                if (isSliding)
                {
                    OnGroundContactLost();
                    return MovementState.Sliding;
                }

                return MovementState.Grounded;
            }

            if (_currentMovementState == MovementState.Falling)
            {
                if (isRising)
                {
                    return MovementState.Rising;
                }

                if (_mover.IsGrounded() && !isSliding)
                {
                    OnGroundContactRegained();
                    return MovementState.Grounded;
                }

                if (isSliding)
                {
                    return MovementState.Sliding;
                }

                return MovementState.Falling;
            }

            if (_currentMovementState == MovementState.Sliding)
            {
                if (isRising)
                {
                    OnGroundContactLost();
                    return MovementState.Rising;
                }

                if (!_mover.IsGrounded())
                {
                    OnGroundContactLost();
                    return MovementState.Falling;
                }

                if (_mover.IsGrounded() && !isSliding)
                {
                    OnGroundContactRegained();
                    return MovementState.Grounded;
                }

                return MovementState.Sliding;
            }

            if (_currentMovementState == MovementState.Rising)
            {
                if (!isRising)
                {
                    if (_mover.IsGrounded() && !isSliding)
                    {
                        OnGroundContactRegained();
                        return MovementState.Grounded;
                    }

                    if (isSliding)
                    {
                        return MovementState.Sliding;
                    }

                    if (!_mover.IsGrounded())
                    {
                        return MovementState.Falling;
                    }
                }

                if (_ceilingDetector != null)
                {
                    if (_ceilingDetector.HitCeiling())
                    {
                        OnCeilingContact();
                        return MovementState.Falling;
                    }
                }

                return MovementState.Rising;
            }

            if (_currentMovementState == MovementState.Jumping)
            {
                if (Time.time - _currentJumpStartTime > PlayerMovementParameters.JumpDuration)
                {
                    return MovementState.Rising;
                }

                if (_jumpKeyWasLetGo)
                {
                    return MovementState.Rising;
                }

                if (_ceilingDetector != null)
                {
                    if (_ceilingDetector.HitCeiling())
                    {
                        OnCeilingContact();
                        return MovementState.Falling;
                    }
                }

                return MovementState.Jumping;
            }

            return MovementState.Falling;
        }

        private void HandleJumping()
        {
            if (_currentMovementState == MovementState.Grounded)
            {
                if ((_jumpKeyIsPressed || _jumpKeyWasPressed) && !_jumpInputIsLocked)
                {
                    OnGroundContactLost();
                    OnJumpStart();

                    _currentMovementState = MovementState.Jumping;
                }
            }
        }

        private void HandleMomentum()
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            Vector3 verticalMomentum = Vector3.zero;
            Vector3 horizontalMomentum = Vector3.zero;

            if (_momentum != Vector3.zero)
            {
                verticalMomentum = VectorMath.ExtractDotVector(_momentum, _playerTransform.up);
                horizontalMomentum = _momentum - verticalMomentum;
            }

            verticalMomentum -= _playerTransform.up * (PlayerMovementParameters.Gravity * Time.deltaTime);

            if (_currentMovementState == MovementState.Grounded &&
                VectorMath.GetDotProduct(verticalMomentum, _playerTransform.up) < 0f)
            {
                verticalMomentum = Vector3.zero;
            }

            if (!IsGrounded())
            {
                Vector3 movementVelocity = CalculateMovementVelocity();

                if (horizontalMomentum.magnitude > PlayerMovementParameters.MovementSpeed)
                {
                    if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                    {
                        movementVelocity =
                            VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);
                    }

                    const float airControlMultiplier = 0.25f;
                    horizontalMomentum += movementVelocity *
                                          (Time.deltaTime * PlayerMovementParameters.AirControlRate *
                                           airControlMultiplier);
                }
                else
                {
                    horizontalMomentum += movementVelocity * (Time.deltaTime * PlayerMovementParameters.AirControlRate);
                    horizontalMomentum =
                        Vector3.ClampMagnitude(horizontalMomentum, PlayerMovementParameters.MovementSpeed);
                }
            }

            if (_currentMovementState == MovementState.Sliding)
            {
                Vector3 pointDownVector =
                    Vector3.ProjectOnPlane(_mover.GetGroundNormal(), _playerTransform.up).normalized;

                Vector3 slopeMovementVelocity = CalculateMovementVelocity();
                slopeMovementVelocity = VectorMath.RemoveDotVector(slopeMovementVelocity, pointDownVector);
                horizontalMomentum += slopeMovementVelocity * Time.fixedDeltaTime;
            }

            horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(horizontalMomentum,
                _currentMovementState == MovementState.Grounded
                    ? PlayerMovementParameters.GroundFriction
                    : PlayerMovementParameters.AirFriction, Time.deltaTime,
                Vector3.zero);

            _momentum = horizontalMomentum + verticalMomentum;

            if (_currentMovementState == MovementState.Sliding)
            {
                _momentum = Vector3.ProjectOnPlane(_momentum, _mover.GetGroundNormal());

                if (VectorMath.GetDotProduct(_momentum, _playerTransform.up) > 0f)
                {
                    _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);
                }

                Vector3 slideDirection =
                    Vector3.ProjectOnPlane(-_playerTransform.up, _mover.GetGroundNormal()).normalized;
                _momentum += slideDirection * (PlayerMovementParameters.SlideGravity * Time.deltaTime);
            }

            if (_currentMovementState == MovementState.Jumping)
            {
                _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);
                _momentum += _playerTransform.up * PlayerMovementParameters.JumpSpeed;
            }

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnJumpStart()
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum += _playerTransform.up * PlayerMovementParameters.JumpSpeed;

            _currentJumpStartTime = Time.time;

            _jumpInputIsLocked = true;

            Jumped?.Invoke(_momentum);

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnGroundContactLost()
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            Vector3 velocity = GetMovementVelocity();

            if (velocity.sqrMagnitude >= 0f && _momentum.sqrMagnitude > 0f)
            {
                Vector3 projectedMomentum = Vector3.Project(_momentum, velocity.normalized);
                float dot = VectorMath.GetDotProduct(projectedMomentum.normalized, velocity.normalized);

                if (projectedMomentum.sqrMagnitude >= velocity.sqrMagnitude && dot > 0f)
                {
                    velocity = Vector3.zero;
                }
                else if (dot > 0f)
                {
                    velocity -= projectedMomentum;
                }
            }

            _momentum += velocity;

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnGroundContactRegained()
        {
            if (Landed != null)
            {
                Vector3 collisionVelocity = _momentum;

                if (PlayerMovementParameters.UseLocalMomentum)
                {
                    collisionVelocity = _playerTransform.localToWorldMatrix * collisionVelocity;
                }

                Landed(collisionVelocity);
            }
        }

        private void OnCeilingContact()
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private bool IsRisingOrFalling()
        {
            Vector3 verticalMomentum = VectorMath.ExtractDotVector(GetMomentum(), _playerTransform.up);

            const float limit = 0.001f;

            return verticalMomentum.magnitude > limit;
        }

        private bool IsGroundTooSteep()
        {
            if (!_mover.IsGrounded())
            {
                return true;
            }

            return Vector3.Angle(_mover.GetGroundNormal(), _playerTransform.up) > PlayerMovementParameters.SlopeLimit;
        }

        public Vector3 GetVelocity()
        {
            return _savedVelocity;
        }

        public Vector3 GetMovementVelocity()
        {
            return _savedMovementVelocity;
        }

        private Vector3 GetMomentum()
        {
            Vector3 worldMomentum = _momentum;

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                worldMomentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            return worldMomentum;
        }

        public bool IsGrounded()
        {
            return _currentMovementState is MovementState.Grounded or MovementState.Sliding;
        }

        public bool IsSliding()
        {
            return _currentMovementState == MovementState.Sliding;
        }

        public void AddMomentum(Vector3 momentum)
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum += momentum;

            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        public void SetMomentum(Vector3 newMomentum)
        {
            if (PlayerMovementParameters.UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * newMomentum;
            }
            else
            {
                _momentum = newMomentum;
            }
        }
    }
}
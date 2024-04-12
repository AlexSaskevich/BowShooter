using CMF;
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    public class PlayerMovement : Controller
    {
        [SerializeField] private Mover _mover;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private CeilingDetector _ceilingDetector; // can be null
        [SerializeField] private Transform _cameraTransform;

        private bool _jumpInputIsLocked;
        private bool _jumpKeyWasPressed;
        private bool _jumpKeyWasLetGo;
        private bool _jumpKeyIsPressed;
        private Vector3 _momentum = Vector3.zero;
        private float _currentJumpStartTime;
        private Vector3 _savedVelocity = Vector3.zero;
        private Vector3 _savedMovementVelocity = Vector3.zero;
        private InputReader _inputReader;
        private ControllerState _currentControllerState = ControllerState.Falling;

        [field: SerializeField] public float MovementSpeed { get; private set; } = 7f;
        [field: SerializeField] public float AirControlRate { get; private set; } = 2f;
        [field: SerializeField] public float JumpSpeed { get; private set; } = 10f;
        [field: SerializeField] public float JumpDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float AirFriction { get; private set; } = 0.5f;
        [field: SerializeField] public float GroundFriction { get; private set; } = 100f;
        [field: SerializeField] public float Gravity { get; private set; } = 30f;
        [field: SerializeField] public float SlideGravity { get; private set; } = 5f;
        [field: SerializeField] public float SlopeLimit { get; private set; } = 80f;
        [field: SerializeField] public bool UseLocalMomentum { get; private set; }

        public enum ControllerState
        {
            Grounded,
            Sliding,
            Falling,
            Rising,
            Jumping
        }

        public void Init(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Update()
        {
            HandleJumpKeyInput();
        }

        private void HandleJumpKeyInput()
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

        private void FixedUpdate()
        {
            ControllerUpdate();
        }

        private void ControllerUpdate()
        {
            _mover.CheckForGround();

            _currentControllerState = DetermineControllerState();

            HandleMomentum();

            HandleJumping();

            Vector3 velocity = Vector3.zero;

            if (_currentControllerState == ControllerState.Grounded)
            {
                velocity = CalculateMovementVelocity();
            }

            Vector3 worldMomentum = _momentum;

            if (UseLocalMomentum)
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

            velocity *= MovementSpeed;

            return velocity;
        }

        private bool IsJumpKeyPressed()
        {
            return _inputReader.IsJumpButtonPressed;
        }

        private ControllerState DetermineControllerState()
        {
            bool isRising = IsRisingOrFalling() && VectorMath.GetDotProduct(GetMomentum(), _playerTransform.up) > 0f;
            bool isSliding = _mover.IsGrounded() && IsGroundTooSteep();

            if (_currentControllerState == ControllerState.Grounded)
            {
                if (isRising)
                {
                    OnGroundContactLost();
                    return ControllerState.Rising;
                }

                if (!_mover.IsGrounded())
                {
                    OnGroundContactLost();
                    return ControllerState.Falling;
                }

                if (isSliding)
                {
                    OnGroundContactLost();
                    return ControllerState.Sliding;
                }

                return ControllerState.Grounded;
            }

            if (_currentControllerState == ControllerState.Falling)
            {
                if (isRising)
                {
                    return ControllerState.Rising;
                }

                if (_mover.IsGrounded() && !isSliding)
                {
                    OnGroundContactRegained();
                    return ControllerState.Grounded;
                }

                if (isSliding)
                {
                    return ControllerState.Sliding;
                }

                return ControllerState.Falling;
            }

            if (_currentControllerState == ControllerState.Sliding)
            {
                if (isRising)
                {
                    OnGroundContactLost();
                    return ControllerState.Rising;
                }

                if (!_mover.IsGrounded())
                {
                    OnGroundContactLost();
                    return ControllerState.Falling;
                }

                if (_mover.IsGrounded() && !isSliding)
                {
                    OnGroundContactRegained();
                    return ControllerState.Grounded;
                }

                return ControllerState.Sliding;
            }

            if (_currentControllerState == ControllerState.Rising)
            {
                if (!isRising)
                {
                    if (_mover.IsGrounded() && !isSliding)
                    {
                        OnGroundContactRegained();
                        return ControllerState.Grounded;
                    }

                    if (isSliding)
                    {
                        return ControllerState.Sliding;
                    }

                    if (!_mover.IsGrounded())
                    {
                        return ControllerState.Falling;
                    }
                }

                if (_ceilingDetector != null)
                {
                    if (_ceilingDetector.HitCeiling())
                    {
                        OnCeilingContact();
                        return ControllerState.Falling;
                    }
                }

                return ControllerState.Rising;
            }

            if (_currentControllerState == ControllerState.Jumping)
            {
                if (Time.time - _currentJumpStartTime > JumpDuration)
                {
                    return ControllerState.Rising;
                }

                if (_jumpKeyWasLetGo)
                {
                    return ControllerState.Rising;
                }

                if (_ceilingDetector != null)
                {
                    if (_ceilingDetector.HitCeiling())
                    {
                        OnCeilingContact();
                        return ControllerState.Falling;
                    }
                }

                return ControllerState.Jumping;
            }

            return ControllerState.Falling;
        }

        private void HandleJumping()
        {
            if (_currentControllerState == ControllerState.Grounded)
            {
                if ((_jumpKeyIsPressed || _jumpKeyWasPressed) && !_jumpInputIsLocked)
                {
                    OnGroundContactLost();
                    OnJumpStart();

                    _currentControllerState = ControllerState.Jumping;
                }
            }
        }

        private void HandleMomentum()
        {
            if (UseLocalMomentum)
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

            verticalMomentum -= _playerTransform.up * (Gravity * Time.deltaTime);

            if (_currentControllerState == ControllerState.Grounded &&
                VectorMath.GetDotProduct(verticalMomentum, _playerTransform.up) < 0f)
            {
                verticalMomentum = Vector3.zero;
            }

            if (!IsGrounded())
            {
                Vector3 movementVelocity = CalculateMovementVelocity();

                if (horizontalMomentum.magnitude > MovementSpeed)
                {
                    if (VectorMath.GetDotProduct(movementVelocity, horizontalMomentum.normalized) > 0f)
                    {
                        movementVelocity =
                            VectorMath.RemoveDotVector(movementVelocity, horizontalMomentum.normalized);
                    }

                    const float airControlMultiplier = 0.25f;
                    horizontalMomentum += movementVelocity * (Time.deltaTime * AirControlRate * airControlMultiplier);
                }
                else
                {
                    horizontalMomentum += movementVelocity * (Time.deltaTime * AirControlRate);
                    horizontalMomentum = Vector3.ClampMagnitude(horizontalMomentum, MovementSpeed);
                }
            }

            if (_currentControllerState == ControllerState.Sliding)
            {
                Vector3 pointDownVector =
                    Vector3.ProjectOnPlane(_mover.GetGroundNormal(), _playerTransform.up).normalized;

                Vector3 slopeMovementVelocity = CalculateMovementVelocity();
                slopeMovementVelocity = VectorMath.RemoveDotVector(slopeMovementVelocity, pointDownVector);
                horizontalMomentum += slopeMovementVelocity * Time.fixedDeltaTime;
            }

            horizontalMomentum = VectorMath.IncrementVectorTowardTargetVector(horizontalMomentum,
                _currentControllerState == ControllerState.Grounded ? GroundFriction : AirFriction, Time.deltaTime,
                Vector3.zero);

            _momentum = horizontalMomentum + verticalMomentum;

            if (_currentControllerState == ControllerState.Sliding)
            {
                _momentum = Vector3.ProjectOnPlane(_momentum, _mover.GetGroundNormal());

                if (VectorMath.GetDotProduct(_momentum, _playerTransform.up) > 0f)
                {
                    _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);
                }

                Vector3 slideDirection =
                    Vector3.ProjectOnPlane(-_playerTransform.up, _mover.GetGroundNormal()).normalized;
                _momentum += slideDirection * (SlideGravity * Time.deltaTime);
            }

            if (_currentControllerState == ControllerState.Jumping)
            {
                _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);
                _momentum += _playerTransform.up * JumpSpeed;
            }

            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnJumpStart()
        {
            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum += _playerTransform.up * JumpSpeed;

            _currentJumpStartTime = Time.time;

            _jumpInputIsLocked = true;

            OnJump?.Invoke(_momentum);

            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnGroundContactLost()
        {
            if (UseLocalMomentum)
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

            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        private void OnGroundContactRegained()
        {
            if (OnLand != null)
            {
                Vector3 collisionVelocity = _momentum;

                if (UseLocalMomentum)
                {
                    collisionVelocity = _playerTransform.localToWorldMatrix * collisionVelocity;
                }

                OnLand(collisionVelocity);
            }
        }

        private void OnCeilingContact()
        {
            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum = VectorMath.RemoveDotVector(_momentum, _playerTransform.up);

            if (UseLocalMomentum)
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

            return Vector3.Angle(_mover.GetGroundNormal(), _playerTransform.up) > SlopeLimit;
        }

        public override Vector3 GetVelocity()
        {
            return _savedVelocity;
        }

        public override Vector3 GetMovementVelocity()
        {
            return _savedMovementVelocity;
        }

        private Vector3 GetMomentum()
        {
            Vector3 worldMomentum = _momentum;

            if (UseLocalMomentum)
            {
                worldMomentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            return worldMomentum;
        }

        public override bool IsGrounded()
        {
            return _currentControllerState is ControllerState.Grounded or ControllerState.Sliding;
        }

        public bool IsSliding()
        {
            return _currentControllerState == ControllerState.Sliding;
        }

        public void AddMomentum(Vector3 momentum)
        {
            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.localToWorldMatrix * _momentum;
            }

            _momentum += momentum;

            if (UseLocalMomentum)
            {
                _momentum = _playerTransform.worldToLocalMatrix * _momentum;
            }
        }

        public void SetMomentum(Vector3 newMomentum)
        {
            if (UseLocalMomentum)
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
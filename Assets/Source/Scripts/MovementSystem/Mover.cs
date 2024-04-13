using CMF;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    public class Mover
    {
        private const float SensorRadiusModifier = 0.8f;

        private readonly Transform _playerTransform;
        private readonly Rigidbody _playerRigidbody;
        private readonly Collider _playerCollider;
        private readonly Sensor _sensor;
        private int _currentLayer;
        private bool _isGrounded;
        private bool _isUsingExtendedSensorRange = true;
        private float _baseSensorRange;
        private Vector3 _currentGroundAdjustmentVelocity = Vector3.zero;

        public Mover(Transform playerTransform, Rigidbody playerRigidbody, Collider playerCollider,
            MoverParameters moverParameters)
        {
            _playerTransform = playerTransform;
            _playerRigidbody = playerRigidbody;
            _playerCollider = playerCollider;
            Parameters = moverParameters;
            _playerRigidbody.freezeRotation = true;
            _playerRigidbody.useGravity = false;

            _sensor = new Sensor(_playerTransform, _playerCollider);
            RecalculateColliderDimensions();
            RecalibrateSensor();
        }

        public MoverParameters Parameters { get; private set; }

        public void DrawDebug()
        {
            if (Parameters.IsInDebugMode)
            {
                _sensor.DrawDebug();
            }
        }

        private void RecalculateColliderDimensions()
        {
            if (_playerCollider is CapsuleCollider capsuleCollider)
            {
                capsuleCollider.height = Parameters.ColliderHeight;
                capsuleCollider.center = Parameters.ColliderOffset * Parameters.ColliderHeight;
                capsuleCollider.radius = Parameters.ColliderThickness / 2f;

                capsuleCollider.center +=
                    new Vector3(0f, Parameters.StepHeightRatio * capsuleCollider.height / 2f, 0f);
                capsuleCollider.height *= 1f - Parameters.StepHeightRatio;

                if (capsuleCollider.height / 2f < capsuleCollider.radius)
                {
                    capsuleCollider.radius = capsuleCollider.height / 2f;
                }
            }

            if (_sensor != null)
            {
                RecalibrateSensor();
            }
        }

        private void RecalibrateSensor()
        {
            _sensor.SetCastOrigin(_playerCollider.bounds.center);
            _sensor.SetCastDirection(Sensor.CastDirection.Down);

            RecalculateSensorLayerMask();

            _sensor.castType = Parameters.SensorType;

            float radius = Parameters.ColliderThickness / 2f * SensorRadiusModifier;

            const float safetyDistanceFactor = 0.001f;

            if (_playerCollider is CapsuleCollider capsuleCollider)
            {
                radius = Mathf.Clamp(radius, safetyDistanceFactor,
                    capsuleCollider.height / 2f * (1f - safetyDistanceFactor));
            }

            Vector3 localScale = _playerTransform.localScale;
            _sensor.sphereCastRadius = radius * localScale.x;

            float length = 0f;
            length += Parameters.ColliderHeight * (1f - Parameters.StepHeightRatio) * 0.5f;
            length += Parameters.ColliderHeight * Parameters.StepHeightRatio;
            _baseSensorRange = length * (1f + safetyDistanceFactor) * localScale.x;
            _sensor.castLength = length * localScale.x;

            _sensor.ArrayRows = Parameters.SensorArrayRows;
            _sensor.arrayRayCount = Parameters.SensorArrayRayCount;
            _sensor.offsetArrayRows = Parameters.SensorArrayRowsAreOffset;
            _sensor.isInDebugMode = Parameters.IsInDebugMode;

            _sensor.calculateRealDistance = true;
            _sensor.calculateRealSurfaceNormal = true;

            _sensor.RecalibrateRaycastArrayPositions();
        }

        private void RecalculateSensorLayerMask()
        {
            int layerMask = 0;
            int objectLayer = _playerTransform.gameObject.layer;

            for (int i = 0; i < 32; i++)
            {
                if (!Physics.GetIgnoreLayerCollision(objectLayer, i))
                {
                    layerMask |= 1 << i;
                }
            }

            if (layerMask == (layerMask | (1 << LayerMask.NameToLayer("Ignore Raycast"))))
            {
                layerMask ^= 1 << LayerMask.NameToLayer("Ignore Raycast");
            }

            _sensor.layermask = layerMask;

            _currentLayer = objectLayer;
        }

        private void Check()
        {
            _currentGroundAdjustmentVelocity = Vector3.zero;

            if (_isUsingExtendedSensorRange)
            {
                _sensor.castLength =
                    _baseSensorRange + Parameters.ColliderHeight * _playerTransform.localScale.x *
                    Parameters.StepHeightRatio;
            }
            else
            {
                _sensor.castLength = _baseSensorRange;
            }

            _sensor.Cast();

            if (!_sensor.HasDetectedHit())
            {
                _isGrounded = false;
                return;
            }

            _isGrounded = true;

            float distance = _sensor.GetDistance();

            Vector3 localScale = _playerTransform.localScale;
            float upperLimit = Parameters.ColliderHeight * localScale.x *
                               (1f - Parameters.StepHeightRatio) * 0.5f;
            float middle = upperLimit +
                           Parameters.ColliderHeight * localScale.x * Parameters.StepHeightRatio;
            float distanceToGo = middle - distance;
            _currentGroundAdjustmentVelocity = _playerTransform.up * (distanceToGo / Time.fixedDeltaTime);
        }

        public void CheckForGround()
        {
            if (_currentLayer != _playerTransform.gameObject.layer)
            {
                RecalculateSensorLayerMask();
            }

            Check();
        }

        public void SetVelocity(Vector3 velocity)
        {
            _playerRigidbody.velocity = velocity + _currentGroundAdjustmentVelocity;
        }

        public bool IsGrounded()
        {
            return _isGrounded;
        }

        public void SetExtendSensorRange(bool isExtended)
        {
            _isUsingExtendedSensorRange = isExtended;
        }

        public void SetColliderHeight(float newColliderHeight)
        {
            if (Parameters.ColliderHeight == newColliderHeight)
            {
                return;
            }

            Parameters.ColliderHeight = newColliderHeight;
            RecalculateColliderDimensions();
        }

        public void SetColliderThickness(float newColliderThickness)
        {
            if (Parameters.ColliderThickness == newColliderThickness)
            {
                return;
            }

            if (newColliderThickness < 0f)
            {
                newColliderThickness = 0f;
            }

            Parameters.ColliderThickness = newColliderThickness;
            RecalculateColliderDimensions();
        }

        public void SetStepHeightRatio(float newStepHeightRatio)
        {
            newStepHeightRatio = Mathf.Clamp(newStepHeightRatio, 0f, 1f);
            Parameters.StepHeightRatio = newStepHeightRatio;
            RecalculateColliderDimensions();
        }

        public Vector3 GetGroundNormal()
        {
            return _sensor.GetNormal();
        }

        public Vector3 GetGroundPoint()
        {
            return _sensor.GetPosition();
        }

        public Collider GetGroundCollider()
        {
            return _sensor.GetCollider();
        }
    }
}
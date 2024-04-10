using System;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    [Serializable]
    internal record FollowingToTargetParameters
    {
        [field: SerializeField] public UpdateType UpdateType { get; private set; } = UpdateType.Update;
        [field: SerializeField] public SmoothType SmoothType { get; private set; } = SmoothType.SmoothDamp;
        [field: SerializeField] public float LerpSpeed { get; private set; } = 20f;
        [field: SerializeField] public float SmoothDampTime { get; private set; } = 0.02f;
        [field: SerializeField] public bool ExtrapolatePosition { get; private set; }
    }

    public class FollowingToTarget : MonoBehaviour
    {
        [SerializeField] private FollowingToTargetParameters _followingToTargetParameters;
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private Transform _follower;

        private Vector3 _currentPosition;
        private Vector3 _localPositionOffset;
        private Vector3 _refVelocity;

        public void Init()
        {
            _currentPosition = _follower.position;
            _localPositionOffset = _follower.localPosition;
        }

        private void OnEnable()
        {
            ResetCurrentPosition();
        }

        private void Update()
        {
            if (_followingToTargetParameters.UpdateType == UpdateType.LateUpdate)
            {
                return;
            }

            SmoothUpdate();
        }

        private void LateUpdate()
        {
            if (_followingToTargetParameters.UpdateType == UpdateType.Update)
            {
                return;
            }

            SmoothUpdate();
        }

        private void SmoothUpdate()
        {
            _currentPosition = Smooth(_currentPosition, _targetTransform.position,
                _followingToTargetParameters.LerpSpeed);
            _follower.position = _currentPosition;
        }

        private Vector3 Smooth(Vector3 start, Vector3 target, float smoothTime)
        {
            //Convert local position offset to world coordinates;
            Vector3 offset = _follower.localToWorldMatrix * _localPositionOffset;

            if (_followingToTargetParameters.ExtrapolatePosition)
            {
                Vector3 difference = target - (start - offset);
                target += difference;
            }

            target += offset;

            return _followingToTargetParameters.SmoothType switch
            {
                SmoothType.Lerp => Vector3.Lerp(start, target, Time.deltaTime * smoothTime),
                SmoothType.SmoothDamp => Vector3.SmoothDamp(start, target, ref _refVelocity,
                    _followingToTargetParameters.SmoothDampTime),
                _ => Vector3.zero
            };
        }

        private void ResetCurrentPosition()
        {
            //Convert local position offset to world coordinates;
            Vector3 offset = _follower.localToWorldMatrix * _localPositionOffset;
            _currentPosition = _targetTransform.position + offset;
        }
    }
}
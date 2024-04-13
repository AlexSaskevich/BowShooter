using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    public class FollowingToTarget
    {
        private readonly Transform _targetTransform;
        private readonly Transform _follower;
        private readonly Vector3 _localPositionOffset;

        private Vector3 _currentPosition;
        private Vector3 _refVelocity;

        public FollowingToTarget(Transform targetTransform, Transform follower,
            FollowingToTargetParameters followingToTargetParameters)
        {
            _targetTransform = targetTransform;
            _follower = follower;
            FollowingToTargetParameters = followingToTargetParameters;
            _currentPosition = _follower.position;
            _localPositionOffset = _follower.localPosition;
        }

        public FollowingToTargetParameters FollowingToTargetParameters { get; private set; }

        public void SmoothUpdate()
        {
            _currentPosition = Smooth(_currentPosition, _targetTransform.position,
                FollowingToTargetParameters.LerpSpeed);
            _follower.position = _currentPosition;
        }

        public void ResetCurrentPosition()
        {
            Vector3 offset = _follower.localToWorldMatrix * _localPositionOffset;
            _currentPosition = _targetTransform.position + offset;
        }

        private Vector3 Smooth(Vector3 start, Vector3 target, float smoothTime)
        {
            Vector3 offset = _follower.localToWorldMatrix * _localPositionOffset;

            if (FollowingToTargetParameters.ExtrapolatePosition)
            {
                Vector3 difference = target - (start - offset);
                target += difference;
            }

            target += offset;

            return FollowingToTargetParameters.SmoothType switch
            {
                SmoothType.Lerp => Vector3.Lerp(start, target, Time.deltaTime * smoothTime),
                SmoothType.SmoothDamp => Vector3.SmoothDamp(start, target, ref _refVelocity,
                    FollowingToTargetParameters.SmoothDampTime),
                _ => Vector3.zero
            };
        }
    }
}
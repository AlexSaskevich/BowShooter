using Reflex.Attributes;
using Source.Scripts.CameraSystem;
using Source.Scripts.Infrastructure;
using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        private CameraRotation _cameraRotation;
        private FollowingToTarget _followingToTarget;
        private Mover _mover;
        private PlayerMovement _playerMovement;

        [Inject]
        private void Inject(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
            _cameraRotation = ComponentContainer.GetComponent<CameraRotation>();
            _followingToTarget = ComponentContainer.GetComponent<FollowingToTarget>();
            _mover = componentContainer.GetComponent<Mover>();
            _playerMovement = componentContainer.GetComponent<PlayerMovement>();
        }

        public IComponentContainer ComponentContainer { get; private set; }

        private void OnEnable()
        {
            _followingToTarget.ResetCurrentPosition();
        }

        private void Update()
        {
            _cameraRotation.Perform();

            if (_followingToTarget.FollowingToTargetParameters.UpdateType == UpdateType.Update)
            {
                _followingToTarget.SmoothUpdate();
            }
            
            _playerMovement.HandleJumpKeyInput();
        }

        private void FixedUpdate()
        {
            _playerMovement.ControllerUpdate();
        }

        private void LateUpdate()
        {
            if (_mover.Parameters.IsInDebugMode)
            {
                _mover.DrawDebug();
            }

            if (_followingToTarget.FollowingToTargetParameters.UpdateType == UpdateType.LateUpdate)
            {
                _followingToTarget.SmoothUpdate();
            }
        }
    }
}
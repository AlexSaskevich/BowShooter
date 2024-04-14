using Reflex.Core;
using Source.Scripts.CameraSystem;
using Source.Scripts.Infrastructure;
using Source.Scripts.Input;
using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class PlayerInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Collider _playerCollider;
        [SerializeField] private Transform _cameraControls;
        [SerializeField] private CameraMouseInputParameters _cameraMouseInputParameters;
        [SerializeField] private FollowingToTargetParameters _followingToTargetParameters;
        [SerializeField] private MoverParameters _moverParameters;
        [SerializeField] private PlayerMovementParameters _playerMovementParameters;
        [SerializeField] private CameraRotationParameters _cameraRotationParameters;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            Cursor.lockState = CursorLockMode.Locked;
            InputReader inputReader = new(_cameraMouseInputParameters);
            Mover mover = new(_playerTransform, _playerRigidbody, _playerCollider, _moverParameters);
            FollowingToTarget followingToTarget = new(_playerTransform, _cameraControls, _followingToTargetParameters);
            PlayerMovement playerMovement = new(inputReader, mover, _playerTransform, _playerMovementParameters,
                _cameraControls);
            CameraRotation cameraRotation = new(inputReader, _cameraControls, _cameraRotationParameters);

            ComponentContainer componentContainer = new();

            componentContainer
                .AddComponent(followingToTarget)
                .AddComponent(cameraRotation)
                .AddComponent(mover)
                .AddComponent(playerMovement);

            containerBuilder
                .AddSingleton(componentContainer, typeof(IComponentContainer))
                .AddSingleton(playerMovement)
                .AddSingleton(mover)
                .AddSingleton(inputReader);
        }
    }
}
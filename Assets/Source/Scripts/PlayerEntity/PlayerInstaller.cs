using Reflex.Core;
using Source.Scripts.CameraSystem;
using Source.Scripts.Infrastructure;
using Source.Scripts.Input;
using Source.Scripts.MovementSystem;
using Source.Scripts.RotationSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class PlayerInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Camera _camera;
        [SerializeField] private Player _player;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            InputHandler inputHandler = new();

            MovementConfig movementConfig = ScriptableObject.CreateInstance<MovementConfig>();
            Movement movement = new(_playerRigidbody, movementConfig, inputHandler, _camera.transform);
            Destroy(movementConfig);


            FirstPersonCameraConfig firstPersonCameraConfig =
                ScriptableObject.CreateInstance<FirstPersonCameraConfig>();
            FirstPersonCamera firstPersonCamera =
                new(_camera, inputHandler, firstPersonCameraConfig, _player.transform);
            Destroy(firstPersonCameraConfig);

            Rotation rotation = new(_player.transform, inputHandler);

            ComponentContainer componentContainer = new();

            componentContainer
                .AddComponent(movement)
                .AddComponent(firstPersonCamera)
                .AddComponent(rotation);

            containerBuilder
                .AddSingleton(inputHandler)
                .AddSingleton(componentContainer, typeof(IComponentContainer));
        }
    }
}
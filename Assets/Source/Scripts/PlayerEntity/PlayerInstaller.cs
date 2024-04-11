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
        [SerializeField] private FollowingToTarget _followingToTarget;
        [SerializeField] private CameraRotation _cameraRotation;
        [SerializeField] private CameraMouseInputParameters _cameraMouseInputParameters;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            InputReader inputReader = new();
            CameraMouseInputReader cameraMouseInputReader = new(inputReader, _cameraMouseInputParameters);
            _followingToTarget.Init();
            _cameraRotation.Init(cameraMouseInputReader);

            ComponentContainer componentContainer = new();

            componentContainer
                .AddComponent(_followingToTarget);

            containerBuilder
                .AddSingleton(componentContainer, typeof(IComponentContainer));
        }
    }
}
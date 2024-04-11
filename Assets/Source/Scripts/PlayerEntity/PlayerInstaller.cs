using Reflex.Core;
using Source.Scripts.CameraSystem;
using Source.Scripts.Infrastructure;
using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class PlayerInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private FollowingToTarget _followingToTarget;
        [SerializeField] private CameraRotation _cameraRotation;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            _followingToTarget.Init();
            _cameraRotation.Init();

            ComponentContainer componentContainer = new();

            componentContainer
                .AddComponent(_followingToTarget);

            containerBuilder
                .AddSingleton(componentContainer, typeof(IComponentContainer));
        }
    }
}
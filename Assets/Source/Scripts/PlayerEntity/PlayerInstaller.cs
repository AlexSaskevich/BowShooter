using Reflex.Core;
using Source.Scripts.Infrastructure;
using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class PlayerInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private FollowingToTarget _followingToTarget;
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private Camera _camera;
        [SerializeField] private Player _player;

        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            _followingToTarget.Init();

            ComponentContainer componentContainer = new();

            componentContainer
                .AddComponent(_followingToTarget);

            containerBuilder
                .AddSingleton(componentContainer, typeof(IComponentContainer));
        }
    }
}
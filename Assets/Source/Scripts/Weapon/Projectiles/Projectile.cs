using System;
using Source.Scripts.CollisionSystem;
using Source.Scripts.HealthSystem;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IEntity
    {
        private bool _isDisposed;
        private IDisposable _disposable;

        [field: SerializeField] public Rigidbody Rigidbody;

        [field: SerializeField] public float Damage { get; private set; }

        // [field: SerializeField] public ProjectileDisposeType ProjectileDisposeType { get; private set; }
        [field: SerializeField] public bool IsNeedSpawnVFXOnDestroy { get; private set; }
        [field: SerializeField] public ParticleSystem DestroyVFX { get; private set; }
        [field: SerializeField] public Collider ColliderForCollisionDetection { get; private set; }

        public IComponentContainer ComponentContainer { get; private set; }

        private void Start()
        {
            Init(null);
        }

        public void Init(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
            new EntityDetector(Rigidbody, OnEntityDetected).StartDetect();
        }

        private void OnEntityDetected(IEntity entity)
        {
            if (entity.ComponentContainer.TryGetComponent(out Health health))
            {
                health.TakeDamage(Damage);
            }
        }
    }
}
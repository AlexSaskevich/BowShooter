using Source.Scripts.CollisionSystem;
using Source.Scripts.HealthSystem;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IEntity
    {
        [field: SerializeField] public Rigidbody Rigidbody;
        [field: SerializeField] public ProjectileConfig Config { get; private set; }
        [field: SerializeField] public Transform Transform { get; private set; }

        public IComponentContainer ComponentContainer { get; private set; }

        public virtual void Init(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
            new EntityDetector(Rigidbody, OnEntityDetected).StartDetect();
        }

        private void OnEntityDetected(IEntity entity)
        {
            if (entity.ComponentContainer.TryGetComponent(out Health health))
            {
                health.TakeDamage(Config.Damage);
            }
        }
    }
}
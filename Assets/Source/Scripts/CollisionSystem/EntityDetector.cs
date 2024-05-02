using System;
using R3;
using R3.Triggers;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.CollisionSystem
{
    public class EntityDetector : CollisionDetector<Collision>
    {
        private readonly Action<IEntity> _detected;
        private readonly Rigidbody _rigidbody;

        public EntityDetector(Rigidbody rigidbody, Action<IEntity> detected)
        {
            _detected = detected;
            _rigidbody = rigidbody;
        }

        protected override Observable<Collision> GetObservable()
        {
            return _rigidbody.OnCollisionEnterAsObservable();
        }

        protected override void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.TryGetComponent(out IEntity entity) == false)
            {
                return;
            }

            _detected?.Invoke(entity);
        }
    }
}
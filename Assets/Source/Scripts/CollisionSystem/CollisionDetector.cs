using System;
using R3;
using UnityEngine;

namespace Source.Scripts.CollisionSystem
{
    public abstract class CollisionDetector<T>
    {
        private IDisposable _disposable;

        ~CollisionDetector()
        {
            StopDetect();
        }

        protected abstract Observable<T> GetObservable();

        public void StartDetect()
        {
            _disposable = GetObservable() switch
            {
                Observable<Collision> collisionObservable => collisionObservable.Subscribe(OnCollisionEnter),
                Observable<Collider> colliderObservable => colliderObservable.Subscribe(OnTriggerEnter),
                _ => _disposable
            };
        }

        public void StopDetect() => _disposable?.Dispose();

        protected virtual void OnCollisionEnter(Collision other)
        {
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
        }
    }
}
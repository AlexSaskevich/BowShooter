using System;
using R3;
using UnityEngine;

namespace Source.Scripts.Weapon.Arrows
{
    [Serializable]
    public class ArrowTorque
    {
        [field: SerializeField] public float Velocity { get; private set; }
        [field: SerializeField] public float AngularVelocityFactor { get; private set; }

        private IDisposable _disposable;

        public void StartAddTorque(Rigidbody rigidbody, Transform arrowTransform)
        {
            _disposable?.Dispose();
            _disposable = Observable.EveryUpdate(UnityFrameProvider.FixedUpdate)
                .Subscribe(_ => OnFixedUpdate(rigidbody, arrowTransform));
        }

        public void StopAddTorque()
        {
            _disposable?.Dispose();
        }

        private void OnFixedUpdate(Rigidbody rigidbody, Transform arrowTransform)
        {
            Vector3 forward = arrowTransform.forward;
            Vector3 cross = Vector3.Cross(forward, rigidbody.velocity.normalized);
            rigidbody.AddTorque(cross * (rigidbody.velocity.magnitude * Velocity));
            rigidbody.AddTorque(-rigidbody.angularVelocity +
                                Vector3.Project(rigidbody.angularVelocity, forward) *
                                (rigidbody.angularVelocity.magnitude * AngularVelocityFactor));
        }
    }
}
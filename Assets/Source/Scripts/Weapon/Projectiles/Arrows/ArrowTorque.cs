using System;
using R3;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles.Arrows
{
    [Serializable]
    public class ArrowTorque
    {
        private IDisposable _disposable;
        private ArrowConfig _arrowConfig;

        public ArrowTorque(ArrowConfig arrowConfig)
        {
            _arrowConfig = arrowConfig;
        }

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
            rigidbody.AddTorque(cross * (rigidbody.velocity.magnitude * _arrowConfig.Velocity));
            rigidbody.AddTorque(-rigidbody.angularVelocity +
                                Vector3.Project(rigidbody.angularVelocity, forward) *
                                (rigidbody.angularVelocity.magnitude * _arrowConfig.AngularVelocityFactor));
        }
    }
}
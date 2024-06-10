using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles.Arrows
{
    public class Arrow : Projectile
    {
        private ArrowTorque _arrowTorque;

        public override void Init(IComponentContainer componentContainer)
        {
            base.Init(componentContainer);

            _arrowTorque = new ArrowTorque((ArrowConfig)Config);
        }

        private void OnDisable()
        {
            _arrowTorque.StopAddTorque();
        }

        public void Load(Transform parent)
        {
            Transform.parent = parent;
            Transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(new Vector3(90, 0, 0)));
            Transform.localScale = Vector3.one;
            Rigidbody.isKinematic = true;
            _arrowTorque.StopAddTorque();
        }

        public void Fly(Vector3 direction, float velocity)
        {
            Transform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.velocity = direction * velocity;
            _arrowTorque.StartAddTorque(Rigidbody, Transform);
        }
    }
}
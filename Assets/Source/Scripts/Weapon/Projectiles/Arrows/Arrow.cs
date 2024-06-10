using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles.Arrows
{
    public class Arrow : Projectile
    {
        private ArrowTorque _arrowTorque;

        [field: SerializeField] public Transform ArrowTransform { get; private set; }

        public override void Init(IComponentContainer componentContainer)
        {
            base.Init(componentContainer);

            _arrowTorque = new ArrowTorque((ArrowConfig)Config);
        }

        public void Load(Transform parent)
        {
            ArrowTransform.parent = parent;
            ArrowTransform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(new Vector3(90, 0, 0)));
            ArrowTransform.localScale = Vector3.one;
            Rigidbody.isKinematic = true;
            _arrowTorque.StopAddTorque();
        }

        public void Fly(Vector3 direction, float velocity)
        {
            ArrowTransform.parent = null;
            Rigidbody.isKinematic = false;
            Rigidbody.velocity = direction * velocity;
            _arrowTorque.StartAddTorque(Rigidbody, ArrowTransform);
        }
    }
}
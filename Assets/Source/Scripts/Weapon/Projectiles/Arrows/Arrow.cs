using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles.Arrows
{
    public class Arrow : Projectile
    {
        [SerializeField] private ArrowTorque _arrowTorque;

        [field: SerializeField] public Transform ArrowTransform { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        public void Load(Transform parent)
        {
            ArrowTransform.parent = parent;
            ArrowTransform.localPosition = Vector3.zero;
            ArrowTransform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
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
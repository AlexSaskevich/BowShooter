using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Arrows
{
    [Serializable]
    public class Arrow
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private ArrowTorque _arrowTorque;

        [field: SerializeField] public Transform ArrowTransform { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        public void Load(Transform parent)
        {
            ArrowTransform.parent = parent;
            ArrowTransform.localPosition = Vector3.zero;
            ArrowTransform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            _rigidbody.isKinematic = true;
            _arrowTorque.StopAddTorque();
        }

        public void Fly(Vector3 direction, float velocity)
        {
            ArrowTransform.parent = null;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = direction * velocity;
            _arrowTorque.StartAddTorque(_rigidbody, ArrowTransform);
        }
    }
}
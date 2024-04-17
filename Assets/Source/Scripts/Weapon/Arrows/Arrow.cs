using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Arrows
{
    [Serializable]
    public class Arrow
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _arrowTransform;
        [SerializeField] private ArrowTorque _arrowTorque;

        [field: SerializeField] public float Speed { get; private set; }

        public void Load(Transform parent)
        {
            _arrowTransform.parent = parent;
            _arrowTransform.localPosition = Vector3.zero;
            _arrowTransform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            _rigidbody.isKinematic = true;
            _arrowTorque.StopAddTorque();
        }

        public void Fly(float velocity)
        {
            _arrowTransform.parent = null;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = _arrowTransform.forward * velocity;
            _arrowTorque.StartAddTorque(_rigidbody, _arrowTransform);
        }
    }
}
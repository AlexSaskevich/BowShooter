using System.Collections.Generic;
using R3;
using R3.Triggers;
using Source.Scripts.PlayerEntity;
using UnityEngine;

namespace Source.Scripts.Environment
{
    public class Tunnel : MonoBehaviour
    {
        [SerializeField] private Collider _collider;

        private readonly List<Rigidbody> _rigidbodies = new();
        private Transform _transform;
        private CompositeDisposable _compositeDisposable;

        private void Start()
        {
            _transform = transform;
            _compositeDisposable = new CompositeDisposable();
            _compositeDisposable.Add(_collider.OnTriggerEnterAsObservable()
                .Where(x => x.TryGetComponent(out Player _))
                .Subscribe(player => _rigidbodies.Add(player.GetComponent<Rigidbody>())));

            _compositeDisposable.Add(_collider.OnTriggerExitAsObservable()
                .Where(x => x.TryGetComponent(out Player _)).Subscribe(player =>
                {
                    Rigidbody rigidbody = player.GetComponent<Rigidbody>();

                    _rigidbodies.Remove(rigidbody);

                    RotateRigidbody(rigidbody, Vector3.up);

                    Vector3 eulerAngles = rigidbody.rotation.eulerAngles;

                    eulerAngles.z = 0f;
                    eulerAngles.x = 0f;

                    rigidbody.MoveRotation(Quaternion.Euler(eulerAngles));
                }));
        }

        private void FixedUpdate()
        {
            foreach (Rigidbody rigidbogy in _rigidbodies)
            {
                Vector3 position = _transform.position;
                Transform rigidbogyTransform = rigidbogy.transform;
                Vector3 center = Vector3.Project(rigidbogyTransform.position - position,
                    position + _transform.forward - position) + position;

                RotateRigidbody(rigidbogy, (center - rigidbogyTransform.position).normalized);
            }
        }

        private void OnDestroy()
        {
            _compositeDisposable?.Dispose();
        }

        private void RotateRigidbody(Rigidbody rigidbody, Vector3 targetDirection)
        {
            Transform rigidbodyTransform = rigidbody.transform;
            targetDirection.Normalize();
            Quaternion rotationDifference = Quaternion.FromToRotation(rigidbodyTransform.up, targetDirection);
            Quaternion endRotation = rotationDifference * rigidbodyTransform.rotation;
            rigidbody.MoveRotation(endRotation);
        }
    }
}
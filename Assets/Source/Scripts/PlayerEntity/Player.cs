using Reflex.Attributes;
using Source.Scripts.CameraSystem;
using Source.Scripts.Infrastructure;
using Source.Scripts.MovementSystem;
using Source.Scripts.RotationSystem;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        private Movement _movement;
        private FirstPersonCamera _firstPersonCamera;
        private Rotation _rotation;

        [Inject]
        private void Inject(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        public IComponentContainer ComponentContainer { get; private set; }

        private void Start()
        {
            _movement = ComponentContainer.GetComponent<Movement>();
            _firstPersonCamera = ComponentContainer.GetComponent<FirstPersonCamera>();
            _rotation = ComponentContainer.GetComponent<Rotation>();
        }

        private void Update() => _rotation.Perform();

        private void FixedUpdate() => _movement.Perform();

        private void LateUpdate() => _firstPersonCamera.Rotate();
    }
}
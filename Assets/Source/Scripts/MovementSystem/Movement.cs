using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    public class Movement
    {
        private readonly Rigidbody _rigidbody;
        private readonly InputHandler _inputHandler;
        private readonly float _moveSpeed;
        private readonly Transform _camera;

        public Movement(Rigidbody rigidbody, MovementConfig movementConfig, InputHandler inputHandler, Transform camera)
        {
            _rigidbody = rigidbody;
            _moveSpeed = movementConfig.MoveSpeed;
            _rigidbody.freezeRotation = true;
            _inputHandler = inputHandler;
            _camera = camera;
        }

        public void Perform()
        {
            Vector2 moveInput = _inputHandler.MoveInput;
            Vector3 movement = (moveInput.y * _camera.forward + moveInput.x * _camera.right).normalized;
            Vector3 velocity = movement * _moveSpeed;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
        }
    }
}
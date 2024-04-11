using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    public class Movement
    {
        private readonly Rigidbody _rigidbody;
        private readonly InputReader _inputReader;
        private readonly float _moveSpeed;
        private readonly Transform _camera;

        public Movement(Rigidbody rigidbody, MovementConfig movementConfig, InputReader inputReader, Transform camera)
        {
            _rigidbody = rigidbody;
            _moveSpeed = movementConfig.MoveSpeed;
            _rigidbody.freezeRotation = true;
            _inputReader = inputReader;
            _camera = camera;
        }

        public void Perform()
        {
            Vector2 moveInput = _inputReader.MoveInput;
            Vector3 movement = (moveInput.y * _camera.forward + moveInput.x * _camera.right).normalized;
            Vector3 velocity = movement * _moveSpeed;
            velocity.y = _rigidbody.velocity.y;
            _rigidbody.velocity = velocity;
        }
    }
}
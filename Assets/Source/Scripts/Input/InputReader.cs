using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Input
{
    public class InputReader : IDisposable
    {
        private readonly CharacterInput _characterInput;

        public InputReader()
        {
            _characterInput = new CharacterInput();
            _characterInput.Player.Enable();
            _characterInput.Player.Move.performed += OnMovePerformed;
            _characterInput.Player.Move.canceled += OnMoveCanceled;
        }

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput => _characterInput.Player.Look.ReadValue<Vector2>();

        public void Dispose()
        {
            _characterInput.Player.Disable();
            _characterInput.Player.Move.performed -= OnMovePerformed;
            _characterInput.Player.Move.canceled -= OnMoveCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext callbackContext)
        {
            MoveInput = callbackContext.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext callbackContext) => MoveInput = Vector3.zero;
    }
}
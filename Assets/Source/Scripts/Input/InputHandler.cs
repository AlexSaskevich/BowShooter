using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Input
{
    public class InputHandler : IDisposable
    {
        private readonly CharacterInput _characterInput;

        public InputHandler()
        {
            _characterInput = new CharacterInput();
            _characterInput.Player.Enable();
            _characterInput.Player.Move.performed += OnMovePerformed;
            _characterInput.Player.Move.canceled += OnMoveCanceled;
            _characterInput.Player.Attack.performed += OnAttackPerformed;
            _characterInput.Player.Attack.canceled += OnAttackCanceled;
        }

        public Vector2 MoveInput { get; private set; }
        public bool IsAttackButtonClicked { get; private set; }
        public Vector2 LookInput => _characterInput.Player.Look.ReadValue<Vector2>();

        public void Dispose()
        {
            _characterInput.Player.Disable();
            _characterInput.Player.Move.performed -= OnMovePerformed;
            _characterInput.Player.Move.canceled -= OnMoveCanceled;
            _characterInput.Player.Attack.performed -= OnAttackPerformed;
            _characterInput.Player.Attack.canceled -= OnAttackCanceled;
        }

        private void OnMovePerformed(InputAction.CallbackContext callbackContext)
        {
            MoveInput = callbackContext.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext callbackContext) => MoveInput = Vector3.zero;

        private void OnAttackPerformed(InputAction.CallbackContext callbackContext)
        {
            IsAttackButtonClicked = callbackContext.ReadValueAsButton();
        }
        
        private void OnAttackCanceled(InputAction.CallbackContext callbackContext)
        {
            IsAttackButtonClicked = callbackContext.ReadValueAsButton();
        }
    }
}
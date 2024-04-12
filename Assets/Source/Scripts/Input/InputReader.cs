using System;
using Source.Scripts.CameraSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.Scripts.Input
{
    public class InputReader : IDisposable
    {
        private readonly CharacterInput _characterInput;
        private readonly CameraMouseInputParameters _cameraMouseInputParameters;

        public InputReader(CameraMouseInputParameters cameraMouseInputParameters)
        {
            _characterInput = new CharacterInput();
            _cameraMouseInputParameters = cameraMouseInputParameters;
            _characterInput.Player.Enable();
            _characterInput.Player.Move.performed += OnMovePerformed;
            _characterInput.Player.Move.canceled += OnMoveCanceled;
        }

        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput => _characterInput.Player.Look.ReadValue<Vector2>();
        public bool IsJumpButtonPressed => _characterInput.Player.Jump.IsPressed();

        public void Dispose()
        {
            _characterInput.Player.Disable();
            _characterInput.Player.Move.performed -= OnMovePerformed;
            _characterInput.Player.Move.canceled -= OnMoveCanceled;
        }

        public float GetHorizontalCameraInput()
        {
            float input = LookInput.x;

            if (Time.timeScale > 0f && Time.deltaTime > 0f)
            {
                input /= Time.deltaTime;
                input *= Time.timeScale;
            }
            else
            {
                input = 0f;
            }

            input *= _cameraMouseInputParameters.MouseInputMultiplier;

            if (_cameraMouseInputParameters.InvertHorizontalInput)
            {
                input *= -1f;
            }

            return input;
        }

        public float GetVerticalCameraInput()
        {
            float input = -LookInput.y;

            if (Time.timeScale > 0f && Time.deltaTime > 0f)
            {
                input /= Time.deltaTime;
                input *= Time.timeScale;
            }
            else
            {
                input = 0f;
            }

            input *= _cameraMouseInputParameters.MouseInputMultiplier;

            if (_cameraMouseInputParameters.InvertVerticalInput)
            {
                input *= -1f;
            }

            return input;
        }

        private void OnMovePerformed(InputAction.CallbackContext callbackContext)
        {
            MoveInput = callbackContext.ReadValue<Vector2>();
        }

        private void OnMoveCanceled(InputAction.CallbackContext callbackContext) => MoveInput = Vector3.zero;
    }
}
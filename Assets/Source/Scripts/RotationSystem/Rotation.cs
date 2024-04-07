using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.RotationSystem
{
    public class Rotation
    {
        private const float SensitivityX = 10f; // todo config

        private readonly InputHandler _inputHandler;
        private readonly Transform _targetTransform;

        public Rotation(Transform targetTransform, InputHandler inputHandler)
        {
            _targetTransform = targetTransform;
            _inputHandler = inputHandler;
        }

        public void Perform()
        {
            _targetTransform.Rotate(Vector3.up * (_inputHandler.LookInput.x * SensitivityX * Time.deltaTime));
        }
    }
}
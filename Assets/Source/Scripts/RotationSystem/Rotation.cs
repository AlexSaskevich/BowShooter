using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.RotationSystem
{
    public class Rotation
    {
        private const float SensitivityX = 10f; // todo config

        private readonly InputReader _inputReader;
        private readonly Transform _targetTransform;

        public Rotation(Transform targetTransform, InputReader inputReader)
        {
            _targetTransform = targetTransform;
            _inputReader = inputReader;
        }

        public void Perform()
        {
            _targetTransform.Rotate(Vector3.up * (_inputReader.LookInput.x * SensitivityX * Time.deltaTime));
        }
    }
}
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    public class FirstPersonCamera
    {
        private readonly Camera _camera;
        private readonly InputReader _inputReader;
        private readonly FirstPersonCameraConfig _config;
        private readonly Transform _playerTransform;
        private float _rotationX;
        private float _rotationY;

        public FirstPersonCamera(Camera camera, InputReader inputReader, FirstPersonCameraConfig config,
            Transform playerTransform)
        {
            _playerTransform = playerTransform;
            _camera = camera;
            _config = config;
            _inputReader = inputReader;
        }

        public void Rotate()
        {
            float mouseX = _inputReader.LookInput.x;
            float mouseY = _inputReader.LookInput.y;

            _rotationX -= mouseY * _config.SensitivityY * Time.deltaTime;
            _rotationX = Mathf.Clamp(_rotationX, _config.MinAngle, _config.MaxAngle);
            _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
            // _playerTransform.Rotate(Vector3.up * (mouseX * _config.SensitivityX * Time.deltaTime));
        }
    }
}
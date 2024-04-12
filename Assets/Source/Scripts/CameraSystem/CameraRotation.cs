using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    public class CameraRotation : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _transform;
        [SerializeField] private CameraRotationParameters _cameraRotationParameters;

        private InputReader _cameraMouseInputReader;
        private float _oldHorizontalInput;
        private float _oldVerticalInput;
        private float _currentXAngle;
        private float _currentYAngle;

        public void Init(InputReader inputReader)
        {
            _cameraMouseInputReader = inputReader;
            _currentXAngle = _transform.localRotation.eulerAngles.x;
            _currentYAngle = _transform.localRotation.eulerAngles.y;

            RotateCamera(0f, 0f);
        }

        private void Update()
        {
            Perform();
        }

        private void Perform()
        {
            float inputHorizontal = _cameraMouseInputReader.GetHorizontalCameraInput();
            float inputVertical = _cameraMouseInputReader.GetVerticalCameraInput();

            RotateCamera(inputHorizontal, inputVertical);
        }

        private void RotateCamera(float newHorizontalInput, float newVerticalInput)
        {
            if (_cameraRotationParameters.SmoothCameraRotation)
            {
                _oldHorizontalInput = Mathf.Lerp(_oldHorizontalInput, newHorizontalInput,
                    Time.deltaTime * _cameraRotationParameters.CameraSmoothingFactor);
                _oldVerticalInput = Mathf.Lerp(_oldVerticalInput, newVerticalInput,
                    Time.deltaTime * _cameraRotationParameters.CameraSmoothingFactor);
            }
            else
            {
                _oldHorizontalInput = newHorizontalInput;
                _oldVerticalInput = newVerticalInput;
            }

            _currentXAngle += _oldVerticalInput * _cameraRotationParameters.CameraSpeed * Time.deltaTime;
            _currentYAngle += _oldHorizontalInput * _cameraRotationParameters.CameraSpeed * Time.deltaTime;

            _currentXAngle = Mathf.Clamp(_currentXAngle, -_cameraRotationParameters.UpperVerticalLimit,
                _cameraRotationParameters.LowerVerticalLimit);

            UpdateRotation();
        }

        private void UpdateRotation()
        {
            _transform.localRotation = Quaternion.Euler(new Vector3(0, _currentYAngle, 0));
            _transform.localRotation = Quaternion.Euler(new Vector3(_currentXAngle, _currentYAngle, 0));
        }
    }
}
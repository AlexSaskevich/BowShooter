using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    public class CameraMouseInputReader
    {
        private readonly InputReader _inputReader;
        private readonly CameraMouseInputParameters _cameraMouseInputParameters;

        public CameraMouseInputReader(InputReader inputReader, CameraMouseInputParameters cameraMouseInputParameters)
        {
            _cameraMouseInputParameters = cameraMouseInputParameters;
            _inputReader = inputReader;
        }

        public float GetHorizontalCameraInput()
        {
            float input = _inputReader.LookInput.x;

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
            float input = -_inputReader.LookInput.y;

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
    }
}
using System;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    [Serializable]
    public record CameraRotationParameters
    {
        [field: SerializeField, Range(0f, 90f)] public float UpperVerticalLimit { get; private set; } = 60f;
        [field: SerializeField, Range(0f, 90f)] public float LowerVerticalLimit { get; private set; } = 60f;
        [field: SerializeField] public float CameraSpeed { get; private set; } = 250f;
        [field: SerializeField] public bool SmoothCameraRotation { get; private set; }
        //Setting this value to '50f' (or above) will result in no smoothing at all;
        //Setting this value to '1f' (or below) will result in very noticable smoothing;
        //For most situations, a value of '25f' is recommended;
        [field: SerializeField, Range(1f, 50f)] public float CameraSmoothingFactor { get; private set; } = 25f;
    }
}
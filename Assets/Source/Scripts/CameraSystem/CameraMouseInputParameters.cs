using System;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    [Serializable]
    public record CameraMouseInputParameters
    {
        [field: SerializeField] public bool InvertHorizontalInput { get; private set; }
        [field: SerializeField] public bool InvertVerticalInput { get; private set; }
        [field: SerializeField] public float MouseInputMultiplier { get; private set; } = 0.01f;
    }
}
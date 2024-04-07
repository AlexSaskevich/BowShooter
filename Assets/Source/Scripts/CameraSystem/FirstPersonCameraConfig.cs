using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    [CreateAssetMenu(menuName = Constants.Configs + Constants.FirstPersonCamera,
        fileName = nameof(FirstPersonCameraConfig), order = 0)]
    public class FirstPersonCameraConfig : ScriptableObject
    {
        [field: SerializeField] public float SensitivityY { get; private set; } = 10;
        [field: SerializeField] public float SensitivityX { get; private set; } = 10;
        [field: SerializeField] public float MinAngle { get; private set; } = -89f;
        [field: SerializeField] public float MaxAngle { get; private set; } = 89f;
    }
}
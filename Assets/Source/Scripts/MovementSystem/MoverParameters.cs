using System;
using CMF;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    [Serializable]
    public record MoverParameters
    {
        [field: SerializeField, Range(0f, 1f)] public float StepHeightRatio { get; set; } = 0.25f;
        [field: SerializeField] public float ColliderHeight { get; set; } = 2f;
        [field: SerializeField] public float ColliderThickness { get; set; } = 1f;
        [field: SerializeField] public Vector3 ColliderOffset { get; private set; } = Vector3.zero;
        [field: SerializeField] public Sensor.CastType SensorType { get; private set; } = Sensor.CastType.Raycast;
        [field: SerializeField] public bool IsInDebugMode { get; private set; }
        [field: SerializeField, Range(1, 5)] public int SensorArrayRows { get; private set; } = 1;
        [field: SerializeField, Range(3, 10)] public int SensorArrayRayCount { get; private set; } = 6;
        [field: SerializeField] public bool SensorArrayRowsAreOffset { get; private set; }
        
    }
}
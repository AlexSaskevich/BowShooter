using System;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    [Serializable]
    public record PlayerMovementParameters
    {
        [field: SerializeField] public float MovementSpeed { get; private set; } = 7f;
        [field: SerializeField] public float AirControlRate { get; private set; } = 2f;
        [field: SerializeField] public float JumpSpeed { get; private set; } = 10f;
        [field: SerializeField] public float JumpDuration { get; private set; } = 0.2f;
        [field: SerializeField] public float AirFriction { get; private set; } = 0.5f;
        [field: SerializeField] public float GroundFriction { get; private set; } = 100f;
        [field: SerializeField] public float Gravity { get; private set; } = 30f;
        [field: SerializeField] public float SlideGravity { get; private set; } = 5f;
        [field: SerializeField] public float SlopeLimit { get; private set; } = 80f;
        [field: SerializeField] public bool UseLocalMomentum { get; private set; }
    }
}
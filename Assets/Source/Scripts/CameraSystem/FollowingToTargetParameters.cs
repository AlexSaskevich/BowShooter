using System;
using Source.Scripts.MovementSystem;
using UnityEngine;

namespace Source.Scripts.CameraSystem
{
    [Serializable]
    public record FollowingToTargetParameters
    {
        [field: SerializeField] public UpdateType UpdateType { get; private set; } = UpdateType.Update;
        [field: SerializeField] public SmoothType SmoothType { get; private set; } = SmoothType.SmoothDamp;
        [field: SerializeField] public float LerpSpeed { get; private set; } = 20f;
        [field: SerializeField] public float SmoothDampTime { get; private set; } = 0.02f;
        [field: SerializeField] public bool ExtrapolatePosition { get; private set; }
    }
}
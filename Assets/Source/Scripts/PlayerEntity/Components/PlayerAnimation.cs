using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.PlayerEntity.Components
{
    public class PlayerAnimation : Animation
    {
        public PlayerAnimation(Animator animator) : base(animator)
        {
        }
    }
}
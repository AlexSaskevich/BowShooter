using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.BotEntity.Bots.Components
{
    public class BotAnimation : Animation
    {
        public readonly int SpeedHash = Animator.StringToHash("Speed");

        public BotAnimation(Animator animator) : base(animator)
        {
        }
    }
}
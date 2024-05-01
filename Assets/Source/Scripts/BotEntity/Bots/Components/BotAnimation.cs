using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.BotEntity.Bots.Components
{
    public class BotAnimation : Animation
    {
        public int SpeedHash { get; private set; } = Animator.StringToHash("Speed");
        public int DieHash { get; private set; } = Animator.StringToHash("Die");

        public BotAnimation(Animator animator) : base(animator)
        {
        }
    }
}
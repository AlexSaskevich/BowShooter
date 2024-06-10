using Source.Scripts.Weapon.Bow.Components.TensionSystem;
using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.Weapon.Bow
{
    public class BowView
    {
        private readonly Animation _animation;
        private readonly int _tensionHash = Animator.StringToHash(nameof(Tension));

        public BowView(Animation animation)
        {
            _animation = animation;
        }

        public void PullBowstring(float tension)
        {
            _animation.SetFloat(_tensionHash, tension);
        }

        public void ReleaseBowstring()
        {
            _animation.SetFloat(_tensionHash, 0);
        }
    }
}
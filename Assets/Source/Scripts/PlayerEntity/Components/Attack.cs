using System;
using Source.Scripts.Weapon.Bow;
using Source.Scripts.Weapon.Bow.TensionSystem;
using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.PlayerEntity.Components
{
    public class Attack : IDisposable
    {
        private readonly Animation _animation;
        private readonly Bow _bow;

        public Attack(Animation animation, Bow bow)
        {
            _animation = animation;
            _bow = bow;
            _bow.Stretched += OnStretched;
            _bow.Shoot += OnShoot;
        }

        public void Dispose()
        {
            _bow.Stretched -= OnStretched;
            _bow.Shoot -= OnShoot;
        }

        private void OnStretched(float tension)
        {
            _animation.SetFloat(Animator.StringToHash(nameof(Tension)), tension);
        }

        private void OnShoot()
        {
            _animation.SetFloat(Animator.StringToHash(nameof(Tension)), 0);
        }
    }
}
using UnityEngine;

namespace Source.Scripts.AnimationSystem
{
    public abstract class Animation
    {
        private readonly Animator _animator;

        protected Animation(Animator animator)
        {
            _animator = animator;
        }

        public void SetFloat(int hash, float value) => _animator.SetFloat(hash, value);

        public void SetTrigger(int hash) => _animator.SetTrigger(hash);
    }
}
using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    [Serializable]
    public class BowstringAnimation
    {
        [field: SerializeField] public float ReleaseDuration { get; private set; } = 1f;
        [field: SerializeField] public Ease ReleaseEase { get; private set; } = Ease.OutElastic;

        private Tween _tween;

        public void PlayReleaseBowstringAnimation(Transform animatedTransform, float targetValue,
            Action finished = null)
        {
            _tween?.Kill();
            _tween = animatedTransform.DOLocalMoveY(targetValue, ReleaseDuration).SetEase(ReleaseEase)
                .OnComplete(() => finished?.Invoke());
        }
    }
}
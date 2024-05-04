using System;
using DG.Tweening;
using UnityEngine;

namespace Source.Scripts.PauseLogic
{
    [Serializable]
    public class Fader
    {
        [field: SerializeField, Range(0, 1)] public float TargetAlpha { get; private set; }
        [field: SerializeField] public float FadeOutDuration { get; private set; } = 1;
        [field: SerializeField] public float FadeInDuration { get; private set; } = 1;

        public void FadeIn(CanvasGroup canvasGroup, Action finished = null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(TargetAlpha, FadeInDuration).OnComplete(() => finished?.Invoke());
        }

        public void FadeOut(CanvasGroup canvasGroup, Action finished = null)
        {
            canvasGroup.DOFade(0, FadeOutDuration).OnComplete(() => finished?.Invoke());
        }
    }
}
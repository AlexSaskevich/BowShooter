using System;
using UnityEngine;

namespace Source.Scripts.PauseLogic
{
    public class PauseMenuView
    {
        private readonly Fader _fader;
        private readonly CanvasGroup _canvasGroup;

        public PauseMenuView(Fader fader, CanvasGroup canvasGroup)
        {
            _fader = fader;
            _canvasGroup = canvasGroup;
        }

        public void Show(Action finished = null)
        {
            _canvasGroup.gameObject.SetActive(true);
            _fader.FadeIn(_canvasGroup, () => finished?.Invoke());
        }

        public void Hide(Action finished = null)
        {
            _fader.FadeOut(_canvasGroup, () =>
            {
                _canvasGroup.gameObject.SetActive(false);
                finished?.Invoke();
            });
        }
    }
}
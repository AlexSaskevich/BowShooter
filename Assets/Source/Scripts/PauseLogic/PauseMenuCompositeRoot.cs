using Reflex.Attributes;
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.PauseLogic
{
    public class PauseMenuCompositeRoot : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private CanvasGroup _canvasGroup;

        [Inject]
        private void Inject(InputReader inputReader)
        {
            PauseMenu pauseMenu = new(inputReader);
            PauseMenuView pauseMenuView = new(_fader, _canvasGroup);
            PauseMenuPresenter pauseMenuPresenter = new(inputReader, pauseMenu, pauseMenuView);
        }
    }
}
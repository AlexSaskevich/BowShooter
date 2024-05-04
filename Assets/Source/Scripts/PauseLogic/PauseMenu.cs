using System;
using Source.Scripts.Input;
using Source.Scripts.PlayerEntity;

namespace Source.Scripts.PauseLogic
{
    public class PauseMenu
    {
        private readonly InputReader _inputReader;

        public PauseMenu(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public event Action Paused;
        public event Action UnPaused;

        public bool IsPaused { get; private set; }

        public void Pause()
        {
            _inputReader.DisableActionMap(nameof(Player));
            IsPaused = true;
            Paused?.Invoke();
        }

        public void UnPause()
        {
            _inputReader.DisableActionMap("UI");
            IsPaused = false;
            UnPaused?.Invoke();
        }
    }
}
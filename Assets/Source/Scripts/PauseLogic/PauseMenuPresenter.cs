using System;
using Source.Scripts.Input;
using Source.Scripts.PlayerEntity;

namespace Source.Scripts.PauseLogic
{
    public class PauseMenuPresenter : IDisposable
    {
        private readonly PauseMenu _pauseMenu;
        private readonly InputReader _inputReader;
        private readonly PauseMenuView _pauseMenuView;

        public PauseMenuPresenter(InputReader inputReader, PauseMenu pauseMenu, PauseMenuView pauseMenuView)
        {
            _inputReader = inputReader;
            _pauseMenu = pauseMenu;
            _pauseMenuView = pauseMenuView;
            _pauseMenu.Paused += OnPaused;
            _pauseMenu.UnPaused += OnUnPaused;
            _inputReader.OpenPauseMenuPerformed += OnOpenPauseMenuPerformed;
            _inputReader.ClosePauseMenuPerformed += OnClosePauseMenuPerformed;
        }

        public void Dispose()
        {
            _pauseMenu.Paused -= OnPaused;
            _pauseMenu.UnPaused -= OnUnPaused;
            _inputReader.OpenPauseMenuPerformed -= OnOpenPauseMenuPerformed;
            _inputReader.ClosePauseMenuPerformed -= OnClosePauseMenuPerformed;
        }

        private void OnOpenPauseMenuPerformed()
        {
            _pauseMenu.Pause();
        }

        private void OnPaused()
        {
            _pauseMenuView.Show(() => _inputReader.EnableActionMap("UI"));
        }

        private void OnClosePauseMenuPerformed()
        {
            _pauseMenu.UnPause();
        }

        private void OnUnPaused()
        {
            _pauseMenuView.Hide(() => _inputReader.EnableActionMap(nameof(Player)));
        }
    }
}
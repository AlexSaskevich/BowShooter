using Reflex.Attributes;
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    public class BowAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _tensionSpeed;

        private InputReader _inputReader;
        private float _currentTension;

        [Inject]
        private void Inject(InputReader inputReader)
        {
            _inputReader = inputReader;
        }

        private void Update()
        {
            if (_inputReader.IsFireButtonPressed)
            {
                _currentTension = TensionCalculator.Calculate(_currentTension, 1, _tensionSpeed);
                _animator.SetFloat("Tension", _currentTension);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                _currentTension = 0;
                _animator.SetFloat("Tension", _currentTension);
                _animator.SetTrigger("Shoot");
            }
        }
    }
}
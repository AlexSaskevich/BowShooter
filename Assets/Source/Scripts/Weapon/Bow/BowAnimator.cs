using Reflex.Attributes;
using Source.Scripts.Input;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    public class BowAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _tensionSpeed;

        private readonly int _tensionHash = Animator.StringToHash(nameof(Tension));

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
                _animator.SetFloat(_tensionHash, _currentTension);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                _currentTension = 0;
                _animator.SetFloat(_tensionHash, _currentTension);
            }
        }
    }
}
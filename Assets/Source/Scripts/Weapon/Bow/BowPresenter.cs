using Reflex.Attributes;
using Source.Scripts.Input;
using Source.Scripts.Weapon.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    public class BowPresenter : MonoBehaviour
    {
        [SerializeField] private Arrow _arrow;
        [SerializeField] private BowView _bowView;
        [SerializeField] private float _tensionSpeed = 1f;
        [SerializeField] private Transform _arrowPoint;

        private Bow _bow;
        private InputReader _inputReader;
        private float _currentTension;

        [Inject]
        private void Inject(InputReader inputReader)
        {
            _inputReader = inputReader;
            _bow = new Bow();
            _arrow.Load(_arrowPoint);
        }

        private void OnEnable()
        {
            _bow.Stretched += OnStretched;
            _bow.Shoot += OnShoot;
        }

        private void Update() // todo debug
        {
            if (_inputReader.IsFireButtonPressed)
            {
                _currentTension = TensionCalculator.Calculate(_currentTension, _bow.Tension.MaxValue, _tensionSpeed);
                _bow.SetTension(_currentTension);
                _arrow.Load(_arrowPoint);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                _bow.ResetTension();
                _arrow.Fly(_arrow.Speed * _currentTension);
                _currentTension = _bow.Tension.MinValue;
            }
        }

        private void OnDisable()
        {
            _bow.Stretched -= OnStretched;
            _bow.Shoot -= OnShoot;
        }

        private void OnStretched(float tension)
        {
            _bowView.PullBowstring(tension);
        }

        private void OnShoot()
        {
            _bowView.ReleaseBowstring();
        }
    }

    internal class TensionCalculator
    {
        public static float Calculate(float currentValue, float targetValue, float speed)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, speed * Time.deltaTime);
            Debug.Log($"{currentValue}");
            return Mathf.Clamp01(currentValue);
        }
    }
}
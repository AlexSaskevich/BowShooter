using System;
using Lean.Pool;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.Infrastructure;
using Source.Scripts.Input;
using Source.Scripts.Weapon.Bow.Components;
using Source.Scripts.Weapon.Bow.Components.TensionSystem;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    public class Bow : MonoBehaviour, IEntity
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _arrowPoint;

        private InputReader _inputReader;
        private BowView _bowView;
        private float _currentTension;
        private Shooting _shooting;
        private Tension _tension;
        private Arrow _currentArrow;

        public event Action Shoot;
        public event Action<float> Stretched;

        [field: SerializeField] public BowConfig BowConfig { get; private set; }
        public IComponentContainer ComponentContainer { get; private set; }

        public void Init(InputReader inputReader, Camera playerCamera)
        {
            _inputReader = inputReader;
            _tension = new Tension();
            _bowView = new BowView(new BotAnimation(_animator));
            SpawnArrow();
            _shooting = new Shooting(playerCamera);
        }

        private void Update()
        {
            if (_inputReader.IsFireButtonPressed)
            {
                _currentTension =
                    TensionCalculator.Calculate(_currentTension, _tension.MaxValue, BowConfig.TensionSpeed);
                _tension.Set(_currentTension);
                _bowView.PullBowstring(_currentTension);
                Stretched?.Invoke(_currentTension);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                _shooting.Perform(_currentArrow, _currentTension);
                _bowView.ReleaseBowstring();
                _tension.Set(_tension.MinValue);
                _currentTension = _tension.MinValue;
                SpawnArrow();
                Shoot?.Invoke();
            }
        }

        private void SpawnArrow()
        {
            _currentArrow = LeanPool.Spawn(BowConfig.DefaultArrow);
            _currentArrow.Load(_arrowPoint);
            _currentArrow.Init(new ComponentContainer());
        }
    }
}
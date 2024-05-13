using System;
using Lean.Pool;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.Infrastructure;
using Source.Scripts.Input;
using Source.Scripts.Weapon.Bow.Components;
using Source.Scripts.Weapon.Bow.TensionSystem;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Scripts.Weapon.Bow
{
    public class Bow : MonoBehaviour, IEntity
    {
        [SerializeField] private Animator _animator;

        [FormerlySerializedAs("_arrow")] [SerializeField]
        private Arrow _arrowPrefab;

        [SerializeField] private float _tensionSpeed = 1f;
        [SerializeField] private Transform _arrowPoint;

        private InputReader _inputReader;
        private BowView _bowView;
        private float _currentTension;
        private Shooting _shooting;
        private Tension _tension;
        private Arrow _currentArrow;

        public event Action Shoot;
        public event Action<float> Stretched;

        public IComponentContainer ComponentContainer { get; private set; }

        public void Init(InputReader inputReader, Camera playerCamera)
        {
            _inputReader = inputReader;
            _tension = new Tension(0, 1);
            _bowView = new BowView(new BotAnimation(_animator));
            SpawnArrow();
            _shooting = new Shooting(playerCamera);
        }

        private void Update()
        {
            if (_inputReader.IsFireButtonPressed)
            {
                _currentTension = TensionCalculator.Calculate(_currentTension, _tension.MaxValue, _tensionSpeed);
                _tension.Set(_currentTension);
                _bowView.PullBowstring(_currentTension);
                Stretched?.Invoke(_currentTension);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                _shooting.Perform(_currentArrow, _currentTension);
                _bowView.ReleaseBowstring();
                _tension.Set(0);
                _currentTension = 0;
                SpawnArrow();
                Shoot?.Invoke();
            }
        }

        private void SpawnArrow()
        {
            _currentArrow = LeanPool.Spawn(_arrowPrefab);
            _currentArrow.Load(_arrowPoint);
            _currentArrow.Init(new ComponentContainer());
        }
    }
}
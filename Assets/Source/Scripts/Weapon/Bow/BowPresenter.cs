using Reflex.Attributes;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.Input;
using Source.Scripts.PlayerEntity.Components;
using Source.Scripts.Weapon.Bow.TensionLogic;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.Weapon.Bow
{
    public class BowPresenter : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Arrow _arrow;
        [SerializeField] private float _tensionSpeed = 1f;
        [SerializeField] private Transform _arrowPoint;
        [SerializeField] private bool _isNeedDebug;

        private Camera _playerCamera;
        private Bow _bow;
        private BowView _bowView;
        private InputReader _inputReader;
        private float _currentTension;
        private Vector3 _targetPosition;
        private Animation _playerAnimation;
        private Animator _playerAnimator;

        [Inject]
        private void Inject(InputReader inputReader, Camera playerCamera, Animator playerAnimator)
        {
            _inputReader = inputReader;
            _playerCamera = playerCamera;
            _bow = new Bow();
            _bowView = new BowView(new BotAnimation(_animator));
            _playerAnimation = new PlayerAnimation(playerAnimator);
            _arrow.Load(_arrowPoint);
        }

        private void OnEnable()
        {
            _bow.Stretched += OnStretched;
            _bow.Shoot += OnShoot;
        }

        private void Update() // todo debug
        {
            if (_isNeedDebug)
            {
                Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                _targetPosition = Physics.Raycast(ray, out RaycastHit raycastHit)
                    ? raycastHit.point
                    : ray.GetPoint(100);

                if (UnityEngine.Input.GetMouseButtonDown(1))
                {
                    _bow.ResetTension();
                    _currentTension = 0;
                    _arrow.Load(_arrowPoint);
                }
            }

            if (_inputReader.IsFireButtonPressed)
            {
                _currentTension = TensionCalculator.Calculate(_currentTension, _bow.Tension.MaxValue, _tensionSpeed);
                _bow.SetTension(_currentTension);
                _arrow.Load(_arrowPoint);
            }
            else if (_inputReader.IsFireButtonWasReleased)
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            _bow.ResetTension();
            Vector3 direction = CalculateDirection();
            float velocity = _arrow.Speed * _currentTension;
            _arrow.Fly(direction.normalized, velocity);
            _currentTension = _bow.Tension.MinValue;
        }

        private Vector3 CalculateDirection()
        {
            Ray ray = _playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            Vector3 targetPosition =
                Physics.Raycast(ray, out RaycastHit raycastHit) ? raycastHit.point : ray.GetPoint(100);

            return targetPosition - _arrow.ArrowTransform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_targetPosition, 0.15f);
        }

        private void OnDisable()
        {
            _bow.Stretched -= OnStretched;
            _bow.Shoot -= OnShoot;
        }

        private void OnStretched(float tension)
        {
            _bowView.PullBowstring(tension);
            _playerAnimation.SetFloat(Animator.StringToHash(nameof(Tension)), tension);
        }

        private void OnShoot()
        {
            _bowView.ReleaseBowstring();
            _playerAnimation.SetFloat(Animator.StringToHash(nameof(Tension)), 0);
        }
    }
}
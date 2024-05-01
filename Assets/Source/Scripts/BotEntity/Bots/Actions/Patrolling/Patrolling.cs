using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Utils;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Actions.Patrolling
{
    public class Patrolling : IAction
    {
        private readonly Movement _movement;
        private readonly Rotation _rotation;
        private readonly PatrolPoint[] _patrolPoints;
        private readonly Transform _movable;
        private readonly NavMeshPathCalculator _navMeshPathCalculator = new();
        private readonly BotAnimation _animation;

        private Transform _currentPoint;
        private int _lastIndex;
        private float _waitTime;
        private float _lastTimeReachedPosition;

        public Patrolling(IComponentContainer componentContainer, PatrolPoint[] patrolPoints)
        {
            _movement = componentContainer.GetComponent<Movement>();
            _rotation = componentContainer.GetComponent<Rotation>();
            _animation = componentContainer.GetComponent<BotAnimation>();
            _movable = _movement.CharacterController.transform;
            _currentPoint = _movable;
            _patrolPoints = patrolPoints;
        }

        public void OnStart()
        {
            _lastTimeReachedPosition = Time.time;
            _waitTime = _patrolPoints[_lastIndex].WaitTime;
            _currentPoint = _patrolPoints[_lastIndex].Point;
        }

        public TaskStatus ExecuteAction()
        {
            Vector3 startPosition = _movable.transform.position;
            Vector3 targetPosition = _currentPoint.position;
            Vector3 directionToNextPoint =
                _navMeshPathCalculator.GetDirectionToNextPoint(startPosition, targetPosition);

            _animation.SetFloat(_animation.SpeedHash, Mathf.Clamp01(_movement.NormalizedSpeed));

            if (Vector3.Distance(startPosition, targetPosition) <= 0.1f)
            {
                _rotation.Perform(_patrolPoints[GetNextPatrolPointIndex()].Point.position - startPosition);

                if (IsWaitingTimeOver())
                {
                    ChangePatrolPoint();
                    _movement.Perform(Vector3.zero);
                    return TaskStatus.Success;
                }
            }
            else
            {
                _rotation.Perform(directionToNextPoint);
                _movement.Perform(directionToNextPoint);
            }

            return TaskStatus.Running;
        }

        private void ChangePatrolPoint()
        {
            _lastIndex = GetNextPatrolPointIndex();
            PatrolPoint patrolPoint = _patrolPoints[_lastIndex];
            _currentPoint = patrolPoint.Point;
            _waitTime = patrolPoint.WaitTime;
            _lastTimeReachedPosition = Time.time;
        }

        private int GetNextPatrolPointIndex() => (_lastIndex + 1) % _patrolPoints.Length;

        private bool IsWaitingTimeOver() => Time.time - _lastTimeReachedPosition >= _waitTime;
    }
}
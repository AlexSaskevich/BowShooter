using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using Source.Scripts.BotLogic.Utils;
using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.BotLogic.Bots.Actions.Patrolling
{
    public class Patrolling : IAction
    {
        private readonly Movement _movement;
        private readonly Rotation _rotation;
        private readonly PatrolPoint[] _patrolPoints;
        private readonly Transform _movable;
        private readonly NavMeshPathCalculator _navMeshPathCalculator = new();
        private readonly Animation _animation;

        private Transform _currentPoint;
        private int _lastIndex;
        private float _waitTime;
        private float _lastTimeReachedPosition;

        private int _speedHash = Animator.StringToHash("Speed");

        public Patrolling(Bot bot, PatrolPoint[] patrolPoints)
        {
            _movement = bot.ComponentContainer.GetComponent<Movement>();
            _rotation = bot.ComponentContainer.GetComponent<Rotation>();
            _animation = bot.ComponentContainer.GetComponent<Animation>();
            _movable = bot.transform;
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

            _animation.SetFloat(_speedHash,  Mathf.Clamp01(_movement.NormalizedSpeed));

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
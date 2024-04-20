using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using Source.Scripts.BotLogic.Utils;
using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Actions.Patrolling
{
    public class Patrolling : IAction
    {
        private readonly Movement _movement;
        private readonly Rotation _rotation;
        private readonly PatrolPoint[] _patrolPoints;
        private readonly Transform _movable;
        private readonly NavMeshPathCalculator _navMeshPathCalculator = new();

        private Transform _currentPoint;
        private int _lastIndex;
        private float _waitTime;
        private float _lastTimeReachedPosition;

        public Patrolling(Bot bot, PatrolPoint[] patrolPoints)
        {
            _movement = bot.ComponentContainer.GetComponent<Movement>();
            _rotation = bot.ComponentContainer.GetComponent<Rotation>();
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

            if (Vector3.Distance(startPosition, targetPosition) <= 0.1f)
            {
                if (IsWaitingTimeOver())
                {
                    ChangePatrolPoint();
                    _movement.Perform(Vector3.zero);
                    return TaskStatus.Success;
                }
            }
            else
            {
                _rotation.Perform(targetPosition - startPosition);
                _movement.Perform(_navMeshPathCalculator.GetDirectionToNextPoint(startPosition, targetPosition));
            }

            return TaskStatus.Running;
        }

        private void ChangePatrolPoint()
        {
            _lastIndex = (_lastIndex + 1) % _patrolPoints.Length;
            PatrolPoint patrolPoint = _patrolPoints[_lastIndex];
            _currentPoint = patrolPoint.Point;
            _waitTime = patrolPoint.WaitTime;
            _lastTimeReachedPosition = Time.time;
        }

        private bool IsWaitingTimeOver() => Time.time - _lastTimeReachedPosition >= _waitTime;
    }
}
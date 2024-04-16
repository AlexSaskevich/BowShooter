using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using Source.Scripts.BotLogic.Utils;
using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Actions
{
    public class MoveToRandomPosition : IAction
    {
        private readonly float _maxAngle;
        private readonly Transform _movableTransform;
        private Vector3 _desiredPoint;
        private readonly Movement _movement;
        private readonly Transform _target;

        private NavMeshPathCalculator _navMeshPathCalculator;
        private InCirclePointFinderByRaycast _pointFinder;

        public MoveToRandomPosition(Bot bot, float maxAngle)
        {
            _movableTransform = bot.transform;
            _maxAngle = maxAngle;
            _movement = bot.ComponentContainer.GetComponent<Movement>();
            _target = bot.Target;
        }

        public void OnStart()
        {
            _navMeshPathCalculator ??= new NavMeshPathCalculator();
            _pointFinder ??= new InCirclePointFinderByRaycast();
            _desiredPoint =
                _pointFinder.FindFreePointInCircle(_movableTransform.position, mainRadius: 4f, deadZoneRadius: 4f);
            _desiredPoint = _navMeshPathCalculator.GetSamplePosition(_desiredPoint);
        }

        public TaskStatus ExecuteAction()
        {
            Vector3 direction =
                _navMeshPathCalculator.GetDirectionToNextPoint(_movableTransform.position, _desiredPoint);
            _movement.Perform(direction);
            Vector3 currentDirection = _movableTransform.forward;
            Vector3 desiredDirection = direction.normalized;

            if (desiredDirection == Vector3.zero || Vector3.Distance(_movableTransform.position, _desiredPoint) < 1f)
            {
                return TaskStatus.Success;
            }

            float rotationSpeed = _maxAngle * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(currentDirection,
                _target.position - _movableTransform.position, rotationSpeed, 0f);
            newDirection.y = 0;
            _movableTransform.rotation = Quaternion.LookRotation(newDirection);
            return TaskStatus.Running;
        }
    }
}
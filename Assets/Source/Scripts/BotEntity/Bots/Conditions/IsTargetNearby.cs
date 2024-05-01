using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Conditions
{
    public class IsTargetNearby : ICondition
    {
        private readonly Transform _target;
        private readonly Transform _self;
        private readonly float _distance;

        public IsTargetNearby(Transform target, Transform self, float distance)
        {
            _target = target;
            _self = self;
            _distance = distance;
        }

        public TaskStatus GetConditionStatus()
        {
            return Vector3.Distance(_self.position, _target.position) <= _distance
                ? TaskStatus.Success
                : TaskStatus.Failure;
        }
    }
}
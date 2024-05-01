using System;
using Source.Scripts.BotEntity.Bots.Actions;
using Source.Scripts.BotEntity.Bots.Actions.Patrolling;
using Source.Scripts.BotEntity.Bots.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Containers;
using Source.Scripts.HealthSystem;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots
{
    [Serializable]
    public class BotBehaviorTreeDataContainer : BehaviorTreeDataContainer
    {
        [SerializeField] private PatrolPointsHelper _patrolPointsHelper;

        public Container<ICondition> GetPatrollingConditionsContainer(IComponentContainer componentContainer)
        {
            ICondition isAlive = new IsAlive(componentContainer.GetComponent<Health>());
            return CreateConditionContainer(isAlive);
        }

        public Container<IAction> GetPatrollingActionsContainer(IComponentContainer componentContainer)
        {
            IAction patrolling = new Patrolling(componentContainer, _patrolPointsHelper.GetPatrolPoints());
            return CreateActionContainer(patrolling);
        }

        public Container<ICondition> GetDeadConditionsContainer(IComponentContainer componentContainer)
        {
            ICondition isDead = new IsDead(componentContainer.GetComponent<Health>());
            return CreateConditionContainer(isDead);
        }

        public Container<IAction> GetDeadActionsContainer(IComponentContainer componentContainer)
        {
            IAction die = new Die(componentContainer);
            return CreateActionContainer(die);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Source.Scripts.BotEntity.Bots.Actions.Patrolling;
using Source.Scripts.BotEntity.Bots.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Containers;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots
{
    [Serializable]
    public class BotBehaviorTreeDataContainer : BehaviorTreeDataContainer
    {
        [SerializeField] public PatrolPointsHelper _patrolPointsHelper;

        public Container<ICondition> GetPatrollingConditionsContainer(IComponentContainer componentContainer)
        {
            ICondition isTrue = new IsTrue();
            return new Container<ICondition>(
                new ReadOnlyCollection<ICondition>(new List<ICondition> { isTrue }));
        }

        public Container<IAction> GetPatrollingActionsContainer(IComponentContainer componentContainer)
        {
            IAction patrolling = new Patrolling(componentContainer, _patrolPointsHelper.GetPatrolPoints());
            return new Container<IAction>(new ReadOnlyCollection<IAction>(new List<IAction> { patrolling }));
        }

        public Container<ICondition> GetFollowingConditionsContainer()
        {
            List<ICondition> conditions = new() { };
            return new Container<ICondition>(new ReadOnlyCollection<ICondition>(conditions));
        }

        public Container<IAction> GetFollowingActionsContainer()
        {
            List<IAction> actions = new() { };
            return new Container<IAction>(new ReadOnlyCollection<IAction>(actions));
        }

        public Container<ICondition> GetDeathConditionsContainer()
        {
            List<ICondition> conditions = new() { };
            return new Container<ICondition>(new ReadOnlyCollection<ICondition>(conditions));
        }

        public Container<IAction> GetDeathActionsContainer()
        {
            List<IAction> actions = new() { };
            return new Container<IAction>(new ReadOnlyCollection<IAction>(actions));
        }

        public Container<ICondition> GetAttackConditionsContainer()
        {
            List<ICondition> conditions = new() { };
            return new Container<ICondition>(new ReadOnlyCollection<ICondition>(conditions));
        }

        public Container<IAction> GetAttackActionsContainer()
        {
            List<IAction> actions = new() { };
            return new Container<IAction>(new ReadOnlyCollection<IAction>(actions));
        }
    }
}
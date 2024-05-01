using System.Collections.ObjectModel;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;

namespace Source.Scripts.BotEntity.Infrastructure.Containers
{
    public abstract class BehaviorTreeDataContainer
    {
        protected Container<IAction> CreateActionContainer(params IAction[] actions)
        {
            return new Container<IAction>(new ReadOnlyCollection<IAction>(actions));
        }

        protected Container<ICondition> CreateConditionContainer(params ICondition[] conditions)
        {
            return new Container<ICondition>(new ReadOnlyCollection<ICondition>(conditions));
        }
    }
}
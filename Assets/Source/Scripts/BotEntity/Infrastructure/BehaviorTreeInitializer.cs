using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Containers;
using Source.Scripts.Infrastructure;

namespace Source.Scripts.BotEntity.Infrastructure
{
    public abstract class BehaviorTreeInitializer
    {
        private readonly BehaviorTree _behaviorTree;

        protected BehaviorTreeInitializer(BehaviorTree behaviorTree)
        {
            _behaviorTree = behaviorTree;
        }

        public void InitBehaviorTree(IComponentContainer componentContainer)
        {
            InitSequences(componentContainer);
            _behaviorTree.EnableBehavior();
        }

        protected void InitSequence(string conditionsVariable, Container<ICondition> conditionsContainer,
            string actionsVariable, Container<IAction> actionsContainer)
        {
            _behaviorTree.SetVariable(conditionsVariable,
                new SharedConditionsContainer { Value = conditionsContainer });
            _behaviorTree.SetVariable(actionsVariable, new SharedActionsContainer { Value = actionsContainer });
        }

        protected abstract void InitSequences(IComponentContainer componentContainer);
    }
}
using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.BotEntity.Infrastructure.Containers;
using Source.Scripts.HealthSystem;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots
{
    public class BotCompositeRoot : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private BehaviorTree _behaviorTree;
        [SerializeField] private Bot _bot;
        [SerializeField] private BotBehaviorTreeDataContainer _botBehaviorTreeDataContainer;

        private void Start()
        {
            IComponentContainer componentContainer = CreateComponentContainer();
            InitBehaviorTree(componentContainer);
        }

        private IComponentContainer CreateComponentContainer()
        {
            ComponentContainer componentContainer = new();
            Movement movement = new(_characterController);
            Rotation rotation = new(_bot.transform);
            BotAnimation animation = new(_animator);
            IHealth health = new Health(100, 100);
            componentContainer
                .AddComponent(movement)
                .AddComponent(rotation)
                .AddComponent(animation)
                .AddComponent(health);

            return componentContainer;
        }

        private void InitBehaviorTree(IComponentContainer componentContainer)
        {
            InitSequence("CanPatrolling",
                _botBehaviorTreeDataContainer.GetPatrollingConditionsContainer(componentContainer),
                "PatrollingActions", _botBehaviorTreeDataContainer.GetPatrollingActionsContainer(componentContainer));
            _behaviorTree.EnableBehavior();
        }

        private void InitSequence(string conditionsVariable, Container<ICondition> conditionsContainer,
            string actionsVariable, Container<IAction> actionsContainer)
        {
            _behaviorTree.SetVariable(conditionsVariable,
                new SharedConditionsContainer { Value = conditionsContainer });
            _behaviorTree.SetVariable(actionsVariable, new SharedActionsContainer { Value = actionsContainer });
        }
    }
}
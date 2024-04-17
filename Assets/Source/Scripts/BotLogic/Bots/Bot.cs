using BehaviorDesigner.Runtime;
using Reflex.Attributes;
using Source.Scripts.BotLogic.Bots.Actions;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Bots.Conditions;
using Source.Scripts.BotLogic.Infrastructure;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using Source.Scripts.BotLogic.Infrastructure.Conditions;
using Source.Scripts.Infrastructure;
using Source.Scripts.PlayerEntity;
using UnityEngine;

namespace Source.Scripts.BotLogic.Bots
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private BehaviorTree _behaviorTree;

        public IComponentContainer ComponentContainer { get; private set; }
        public Transform Target { get; private set; }
        
        [Inject]
        private void Inject(Player player)
        {
            Target = player.transform;

            ComponentContainer = new ComponentContainer();

            Movement movement = new(_characterController);

            ComponentContainer.AddComponent(movement);


            MoveToRandomPosition moveToRandomPosition = new(this, maxAngle: 2f);
            Chase chase = new(this, movement);
            IsTargetNearby isTargetNearby = new(player.transform, transform, distance: 4f);

            Container<IAction> actionsContainer = new(new IAction[] { moveToRandomPosition });
            Container<ICondition> conditionsContainer = new(new ICondition[] { isTargetNearby });

            _behaviorTree.SetVariable("ActionsContainer", new SharedActionsContainer { Value = actionsContainer });
            _behaviorTree.SetVariable("ConditionsContainer",
                new SharedConditionsContainer { Value = conditionsContainer });

            _behaviorTree.EnableBehavior();
        }
    }
}
using System.Linq;
using BehaviorDesigner.Runtime;
using EasyButtons;
using Reflex.Attributes;
using Source.Scripts.BotLogic.Bots.Actions;
using Source.Scripts.BotLogic.Bots.Actions.Patrolling;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Bots.Conditions;
using Source.Scripts.BotLogic.Infrastructure;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using Source.Scripts.BotLogic.Infrastructure.Conditions;
using Source.Scripts.Infrastructure;
using Source.Scripts.PlayerEntity;
using UnityEditor;
using UnityEngine;
using Animation = Source.Scripts.AnimationSystem.Animation;

namespace Source.Scripts.BotLogic.Bots
{
    public class Bot : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private BehaviorTree _behaviorTree;
        [SerializeField] private PatrolPoint[] _patrolPoints;
        [SerializeField] private Transform[] _transforms;
        [SerializeField] private bool _isNeedShowPointNames = true;

        public IComponentContainer ComponentContainer { get; private set; }
        public Transform Target { get; private set; }

        [Inject]
        private void Inject(Player player)
        {
            Target = player.transform;

            ComponentContainer = new ComponentContainer();

            Movement movement = new(_characterController);
            Rotation rotation = new(transform);
            Animation animation = new(_animator);

            ComponentContainer
                .AddComponent(movement)
                .AddComponent(rotation)
                .AddComponent(animation);

            IsTargetNearby isTargetNearby = new(player.transform, transform, distance: 4f);
            Container<ICondition> conditionsContainer = new(new ICondition[] { isTargetNearby });

            Container<IAction> actionsContainer = CreateActions(this);

            _behaviorTree.SetVariable("ActionsContainer", new SharedActionsContainer { Value = actionsContainer });
            _behaviorTree.SetVariable("ConditionsContainer",
                new SharedConditionsContainer { Value = conditionsContainer });

            _behaviorTree.EnableBehavior();
        }

        private Container<IAction> CreateActions(Bot bot)
        {
            Movement movement = bot.ComponentContainer.GetComponent<Movement>();

            MoveToRandomPosition moveToRandomPosition = new(this, maxAngle: 2f);
            Chase chase = new(this, movement);
            Patrolling patrolling = new(bot, _patrolPoints);
            Container<IAction> actionsContainer = new(new IAction[] { patrolling });
            return actionsContainer;
        }

        [Button]
        private void SetPatrolPoints()
        {
            _patrolPoints = new PatrolPoint[_transforms.Length];

            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                PatrolPoint patrolPoint = _patrolPoints[i];
                patrolPoint.WaitTime = 1f;
                patrolPoint.Point = _transforms[i];
                _patrolPoints[i] = patrolPoint;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector3 previousPoint = _patrolPoints.First().Point.position;

            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                Gizmos.DrawLine(previousPoint, _patrolPoints[i].Point.position);
                previousPoint = _patrolPoints[i].Point.position;

                if (_isNeedShowPointNames == false)
                {
                    continue;
                }
                
                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                Handles.Label(_patrolPoints[i].Point.position, _patrolPoints[i].Point.gameObject.name, style);
            }

            if (_patrolPoints.Length > 1)
            {
                Gizmos.DrawLine(previousPoint, _patrolPoints.First().Point.position);
            }
        }
    }
}
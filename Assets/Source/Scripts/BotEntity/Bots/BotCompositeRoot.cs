using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Bots.Components;
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
            _bot.Init(componentContainer);
            BotBehaviorTreeInitializer botBehaviorTreeInitializer = new(_behaviorTree, _botBehaviorTreeDataContainer);
            botBehaviorTreeInitializer.InitBehaviorTree(componentContainer);
        }

        private IComponentContainer CreateComponentContainer()
        {
            ComponentContainer componentContainer = new();
            Movement movement = new(_characterController);
            Rotation rotation = new(_bot.transform);
            BotAnimation animation = new(_animator);
            Health health = new(100, 100);
            Ragdoll ragdoll = new(_bot.gameObject);
            componentContainer
                .AddComponent(movement)
                .AddComponent(rotation)
                .AddComponent(animation)
                .AddComponent(health)
                .AddComponent(ragdoll);

            return componentContainer;
        }
    }
}
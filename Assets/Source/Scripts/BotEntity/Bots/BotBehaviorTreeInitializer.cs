using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Infrastructure;
using Source.Scripts.Infrastructure;

namespace Source.Scripts.BotEntity.Bots
{
    public class BotBehaviorTreeInitializer : BehaviorTreeInitializer
    {
        private const string CanPatrolling = nameof(CanPatrolling);
        private const string PatrollingActions = nameof(PatrollingActions);
        private const string IsDead = nameof(IsDead);
        private const string DeadActions = nameof(DeadActions);

        private readonly BotBehaviorTreeDataContainer _botBehaviorTreeDataContainer;

        public BotBehaviorTreeInitializer(BehaviorTree behaviorTree,
            BotBehaviorTreeDataContainer botBehaviorTreeDataContainer) : base(behaviorTree)
        {
            _botBehaviorTreeDataContainer = botBehaviorTreeDataContainer;
        }

        protected override void InitSequences(IComponentContainer componentContainer)
        {
            InitSequence(CanPatrolling,
                _botBehaviorTreeDataContainer.GetPatrollingConditionsContainer(componentContainer), PatrollingActions,
                _botBehaviorTreeDataContainer.GetPatrollingActionsContainer(componentContainer));

            InitSequence(IsDead, _botBehaviorTreeDataContainer.GetDeadConditionsContainer(componentContainer),
                DeadActions, _botBehaviorTreeDataContainer.GetDeadActionsContainer(componentContainer));
        }
    }
}
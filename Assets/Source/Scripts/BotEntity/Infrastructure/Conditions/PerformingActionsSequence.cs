using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Infrastructure.Actions;

namespace Source.Scripts.BotEntity.Infrastructure.Conditions
{
    public class PerformingActionsSequence : Action
    {
        public SharedActionsContainer SharedActionsContainer;

        private int _lastIndex;

        public override void OnStart()
        {
            _lastIndex = 0;

            foreach (IAction valueGameAction in SharedActionsContainer.Value.Items)
            {
                valueGameAction.OnStart();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (SharedActionsContainer.Value.Items.Count == 0)
            {
                return TaskStatus.Success;
            }

            TaskStatus currentTaskStatus = TaskStatus.Inactive;

            for (int i = _lastIndex; i < SharedActionsContainer.Value.Items.Count; i++)
            {
                _lastIndex = i;
                currentTaskStatus = SharedActionsContainer.Value.Items[i].ExecuteAction();

                if (currentTaskStatus is TaskStatus.Running or TaskStatus.Failure)
                {
                    break;
                }
            }

            return currentTaskStatus;
        }

        public override void OnEnd()
        {
            foreach (IAction valueGameAction in SharedActionsContainer.Value.Items)
            {
                valueGameAction.OnExit();
            }
        }
    }
}
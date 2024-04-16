using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotLogic.Infrastructure.Actions
{
    public class PerformingActions : Action
    {
        public SharedActionsContainer SharedActionsContainer;

        public override void OnStart()
        {
            foreach (IAction action in SharedActionsContainer.Value.Items)
            {
                action.OnStart();
            }
        }

        public override TaskStatus OnUpdate()
        {
            if (SharedActionsContainer.Value.Items.Count == 0)
            {
                return TaskStatus.Success;
            }

            TaskStatus taskStatus = TaskStatus.Inactive;

            foreach (IAction action in SharedActionsContainer.Value.Items)
            {
                TaskStatus executeActionStatus = action.ExecuteAction();

                switch (executeActionStatus)
                {
                    case TaskStatus.Inactive:
                        break;
                    case TaskStatus.Failure:
                        taskStatus = executeActionStatus;
                        break;
                    case TaskStatus.Success:
                        if (taskStatus != TaskStatus.Running && taskStatus != TaskStatus.Failure)
                            taskStatus = executeActionStatus;
                        break;
                    case TaskStatus.Running:
                        if (taskStatus != TaskStatus.Failure)
                            taskStatus = executeActionStatus;
                        break;
                }
            }

            return taskStatus;
        }

        public override void OnEnd()
        {
            foreach (IAction action in SharedActionsContainer.Value.Items)
            {
                action.OnExit();
            }
        }
    }
}
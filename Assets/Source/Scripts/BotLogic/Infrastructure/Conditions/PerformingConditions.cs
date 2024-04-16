using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotLogic.Infrastructure.Conditions
{
    public class PerformingConditions : Conditional
    {
        public SharedConditionsContainer SharedConditionsContainer;

        public override TaskStatus OnUpdate()
        {
            foreach (ICondition condition in SharedConditionsContainer.Value.Items)
            {
                if (condition.GetConditionStatus() == TaskStatus.Failure)
                {
                    return TaskStatus.Failure;
                }
            }

            return TaskStatus.Success;
        }
    }
}
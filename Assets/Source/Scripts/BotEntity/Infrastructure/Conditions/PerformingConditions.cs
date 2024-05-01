using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotEntity.Infrastructure.Conditions
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
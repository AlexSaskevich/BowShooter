using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Infrastructure.Conditions;

namespace Source.Scripts.BotEntity.Bots.Conditions
{
    public class IsTrue : ICondition
    {
        public TaskStatus GetConditionStatus()
        {
            return TaskStatus.Success;
        }
    }
}
using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotEntity.Infrastructure.Conditions
{
    public interface ICondition
    {
        public TaskStatus GetConditionStatus();
    }
}
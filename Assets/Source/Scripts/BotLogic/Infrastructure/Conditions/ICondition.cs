using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotLogic.Infrastructure.Conditions
{
    public interface ICondition
    {
        public TaskStatus GetConditionStatus();
    }
}
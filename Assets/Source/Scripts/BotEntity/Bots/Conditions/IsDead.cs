using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.HealthSystem;

namespace Source.Scripts.BotEntity.Bots.Conditions
{
    public class IsDead : ICondition
    {
        private readonly Health _health;

        public IsDead(Health health)
        {
            _health = health;
        }

        public TaskStatus GetConditionStatus()
        {
            return _health.CurrentValue <= 0 ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
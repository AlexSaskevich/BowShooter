using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using Source.Scripts.HealthSystem;

namespace Source.Scripts.BotEntity.Bots.Conditions
{
    public class IsAlive : ICondition
    {
        private readonly IHealth _health;

        public IsAlive(IHealth health)
        {
            _health = health;
        }

        public TaskStatus GetConditionStatus()
        {
            return _health.CurrentValue <= 0 ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
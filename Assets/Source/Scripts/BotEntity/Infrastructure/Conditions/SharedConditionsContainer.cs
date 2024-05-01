using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Infrastructure.Containers;

namespace Source.Scripts.BotEntity.Infrastructure.Conditions
{
    public class SharedConditionsContainer : SharedVariable<Container<ICondition>>
    {
        public static implicit operator SharedConditionsContainer(Container<ICondition> value) =>
            new() { Value = value };
    }
}
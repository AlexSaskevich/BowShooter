using BehaviorDesigner.Runtime;

namespace Source.Scripts.BotLogic.Infrastructure.Conditions
{
    public class SharedConditionsContainer : SharedVariable<Container<ICondition>>
    {
        public static implicit operator SharedConditionsContainer(Container<ICondition> value) =>
            new() { Value = value };
    }
}
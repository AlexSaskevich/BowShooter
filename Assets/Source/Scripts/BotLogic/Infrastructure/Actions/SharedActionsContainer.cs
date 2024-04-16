using BehaviorDesigner.Runtime;

namespace Source.Scripts.BotLogic.Infrastructure.Actions
{
    public class SharedActionsContainer : SharedVariable<Container<IAction>>
    {
        public static implicit operator SharedActionsContainer(Container<IAction> value) => new() { Value = value };
    }
}
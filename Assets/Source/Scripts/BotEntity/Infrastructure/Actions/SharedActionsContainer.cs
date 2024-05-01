using BehaviorDesigner.Runtime;
using Source.Scripts.BotEntity.Infrastructure.Containers;

namespace Source.Scripts.BotEntity.Infrastructure.Actions
{
    public class SharedActionsContainer : SharedVariable<Container<IAction>>
    {
        public static implicit operator SharedActionsContainer(Container<IAction> value) => new() { Value = value };
    }
}
using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.Infrastructure;

namespace Source.Scripts.BotEntity.Bots.Actions
{
    public class Die : IAction
    {
        private readonly BotAnimation _animation;
        private readonly Ragdoll _ragdoll;

        public Die(IComponentContainer componentContainer)
        {
            _animation = componentContainer.GetComponent<BotAnimation>();
            _ragdoll = componentContainer.GetComponent<Ragdoll>();
        }

        public void OnStart()
        {
            _animation.Disable();
            _ragdoll.Enable();
        }

        public TaskStatus ExecuteAction()
        {
            return TaskStatus.Running;
        }
    }
}
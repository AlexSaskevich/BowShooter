using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.Infrastructure;

namespace Source.Scripts.BotEntity.Bots.Actions
{
    public class Die : IAction
    {
        private readonly BotAnimation _animation;

        public Die(IComponentContainer componentContainer)
        {
            _animation = componentContainer.GetComponent<BotAnimation>();
        }

        public void OnStart()
        {
            _animation.SetTrigger(_animation.DieHash);
        }

        public TaskStatus ExecuteAction()
        {
            return TaskStatus.Running;
        }
    }
}
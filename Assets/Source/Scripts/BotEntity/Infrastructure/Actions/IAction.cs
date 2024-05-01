using BehaviorDesigner.Runtime.Tasks;

namespace Source.Scripts.BotEntity.Infrastructure.Actions
{
    public interface IAction
    {
        public void OnStart()
        {
        }

        public TaskStatus ExecuteAction();

        public void OnExit()
        {
        }
    }
}
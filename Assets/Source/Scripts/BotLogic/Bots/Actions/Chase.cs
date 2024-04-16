using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotLogic.Bots.Components;
using Source.Scripts.BotLogic.Infrastructure.Actions;
using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Actions
{
    public class Chase : IAction
    {
        private readonly Transform _target;
        private readonly Transform _botTransform;
        private readonly Movement _movement;

        public Chase(Bot bot, Movement movement)
        {
            _target = bot.Target;
            _movement = movement;
            _botTransform = bot.transform;
        }

        public TaskStatus ExecuteAction()
        {
            if (_target == null)
            {
                return TaskStatus.Failure;
            }

            Vector3 direction = _target.position - _botTransform.position;
            _movement.Perform(direction.normalized);

            return TaskStatus.Running;
        }
    }
}
using BehaviorDesigner.Runtime.Tasks;
using Source.Scripts.BotEntity.Bots.Components;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Actions
{
    public class Chase : IAction
    {
        private readonly Transform _target;
        private readonly Transform _botTransform;
        private readonly Movement _movement;

        public Chase(Bot bot, Transform target, IComponentContainer componentContainer)
        {
            _target = target;
            _movement = componentContainer.GetComponent<Movement>();
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
using EasyButtons;
using Source.Scripts.HealthSystem;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots
{
    public class Bot : MonoBehaviour, IEntity
    {
        public IComponentContainer ComponentContainer { get; private set; }

        public void Init(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        [Button]
        private void TakeDamage(float damage)
        {
            ComponentContainer.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
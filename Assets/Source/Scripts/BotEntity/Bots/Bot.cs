using EasyButtons;
using Source.Scripts.BotEntity.Bots.Components;
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

        [Button]
        private void EnableRagdoll()
        {
            ComponentContainer.GetComponent<Ragdoll>().Enable();
        }

        [Button]
        private void DisableRagdoll()
        {
            ComponentContainer.GetComponent<Ragdoll>().Disable();
        }
    }
}
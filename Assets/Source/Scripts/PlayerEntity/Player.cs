using Reflex.Attributes;
using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.PlayerEntity
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Inject(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        public IComponentContainer ComponentContainer { get; private set; }
    }
}
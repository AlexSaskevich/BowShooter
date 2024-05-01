using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots
{
    public class Bot : MonoBehaviour, IEntity
    {
        public IComponentContainer ComponentContainer { get; private set; }
    }
}
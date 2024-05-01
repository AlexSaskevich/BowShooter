using System;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Actions.Patrolling
{
    [Serializable]
    public struct PatrolPoint
    {
        public float WaitTime;
        public Transform Point;
    }
}
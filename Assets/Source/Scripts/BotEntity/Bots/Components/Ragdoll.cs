using System.Collections.Generic;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Components
{
    public class Ragdoll
    {
        private readonly List<Rigidbody> _rigidbodies;

        public Ragdoll(GameObject target)
        {
            _rigidbodies = new List<Rigidbody>(target.GetComponentsInChildren<Rigidbody>());
            Disable();
        }

        public void Enable()
        {
            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = false;
            }
        }

        public void Disable()
        {
            foreach (Rigidbody rigidbody in _rigidbodies)
            {
                rigidbody.isKinematic = true;
            }
        }
    }
}
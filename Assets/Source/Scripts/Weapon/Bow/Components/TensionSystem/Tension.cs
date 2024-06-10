using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow.Components.TensionSystem
{
    public class Tension
    {
        public float MinValue { get; private set; } = 0f;
        public float MaxValue { get; private set; } = 1f;
        public float CurrentValue { get; private set; }

        public void Set(float value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CurrentValue = Mathf.Clamp01(value);
        }
    }
}
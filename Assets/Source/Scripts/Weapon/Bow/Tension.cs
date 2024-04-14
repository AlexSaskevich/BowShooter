using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    public class Tension
    {
        public Tension(float minValue, float maxValue)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        public float MinValue { get; private set; }
        public float MaxValue { get; private set; }
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
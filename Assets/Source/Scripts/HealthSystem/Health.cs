using System;
using UnityEngine;

namespace Source.Scripts.HealthSystem
{
    public class Health : IHealth, IDamageable
    {
        public Health(float currentValue, float defaultValue)
        {
            CurrentValue = currentValue;
            DefaultValue = defaultValue;
        }

        public float CurrentValue { get; private set; }
        public float DefaultValue { get; private set; }

        public void TakeDamage(float damage)
        {
            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(damage));
            }

            CurrentValue = Mathf.Clamp(CurrentValue - damage, 0, DefaultValue);

            if (CurrentValue <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.LogError("Dead");
        }
    }
}
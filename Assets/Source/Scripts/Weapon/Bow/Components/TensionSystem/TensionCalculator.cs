using UnityEngine;

namespace Source.Scripts.Weapon.Bow.Components.TensionSystem
{
    public class TensionCalculator
    {
        public static float Calculate(float currentValue, float targetValue, float speed)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, speed * Time.deltaTime);
            return Mathf.Clamp01(currentValue);
        }
    }
}
using System;
using Source.Scripts.Weapon.Bow.TensionLogic;

namespace Source.Scripts.Weapon.Bow
{
    public class Bow : IShootable
    {
        private const float MinTension = 0;
        private const float MaxTension = 1;

        public event Action<float> Stretched;
        public event Action Shoot;
       
        public Tension Tension { get; } = new(MinTension, MaxTension);

        public void SetTension(float tension)
        {
            if (tension < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(tension));
            }

            Tension.Set(tension);
            Stretched?.Invoke(Tension.CurrentValue);
        }

        public void ResetTension()
        {
            Tension.Set(Tension.MinValue);
            Shoot?.Invoke();
        }
    }
}
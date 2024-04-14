using System;

namespace Source.Scripts.Weapon
{
    public interface IShootable : IWeapon
    {
        public event Action Shoot;
    }
}
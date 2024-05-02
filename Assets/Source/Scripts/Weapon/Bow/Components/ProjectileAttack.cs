using Lean.Pool;
using Source.Scripts.Weapon.Projectiles;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow.Components
{
    public class ProjectileAttack
    {
        private readonly Transform _startPoint;
        private readonly Projectile _projectile;
        private readonly ForceMode _forceMode;
        private readonly float _force;

        public ProjectileAttack(Transform startPoint, Projectile projectile,
            ForceMode forceMode = ForceMode.Impulse, float force = 10)

        {
            _startPoint = startPoint;
            _projectile = projectile;
            _forceMode = forceMode;
            _force = force;
        }

        public void Perform()
        {
            Projectile projectile = LeanPool.Spawn(_projectile, _startPoint.position, Quaternion.identity);
        }
    }
}
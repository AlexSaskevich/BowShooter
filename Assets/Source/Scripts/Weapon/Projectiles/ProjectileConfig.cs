using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles
{
    [CreateAssetMenu(menuName = "Configs/", fileName = "ProjectileConfig", order = 0)]
    public abstract class ProjectileConfig : ScriptableObject
    {
        public abstract float Damage { get; protected set; }
    }
}
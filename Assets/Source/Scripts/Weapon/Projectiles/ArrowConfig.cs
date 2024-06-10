using Sirenix.OdinInspector;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Projectiles
{
    [CreateAssetMenu(menuName = "Configs/ArrowConfig", fileName = "ArrowConfig", order = 0)]
    public class ArrowConfig : ProjectileConfig
    {
        [field: SerializeField, MinValue(1)] public override float Damage { get; protected set; }
        [field: SerializeField, MinValue(1)] public float Speed { get; private set; } = 60;

        [field: SerializeField, FoldoutGroup(nameof(ArrowTorque))]
        public float Velocity { get; private set; } = 5f;

        [field: SerializeField, FoldoutGroup(nameof(ArrowTorque))]
        public float AngularVelocityFactor { get; private set; } = 0.5f;
    }
}
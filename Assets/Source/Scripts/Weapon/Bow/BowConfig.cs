using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    [CreateAssetMenu(menuName = "Configs/BowConfig", fileName = "BowConfig", order = 0)]
    public class BowConfig : ScriptableObject
    {
        [field: SerializeField] public float TensionSpeed { get; private set; } = 1f;
        [field: SerializeField] public int ArrowCountPerShoot { get; private set; } = 1;
        [field: SerializeField] public Arrow DefaultArrow { get; private set; }
    }
}
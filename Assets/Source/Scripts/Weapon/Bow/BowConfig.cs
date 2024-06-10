using Sirenix.OdinInspector;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    [CreateAssetMenu(menuName = "Configs/BowConfig", fileName = "BowConfig", order = 0)]
    public class BowConfig : ScriptableObject
    {
        [field: SerializeField, MinValue(1)] public float TensionSpeed { get; private set; } = 1f;
        [field: SerializeField, MinValue(1)] public int ArrowCountPerShoot { get; private set; } = 1;

        [field: SerializeField, ShowIf("@ArrowCountPerShoot > 1")]
        public float DelayBetweenArrow { get; private set; }

        [field: SerializeField] public bool IsSingleShooting { get; private set; }
        [field: SerializeField] public Arrow DefaultArrow { get; private set; }
    }
}
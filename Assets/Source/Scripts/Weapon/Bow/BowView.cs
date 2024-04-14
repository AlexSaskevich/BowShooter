using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    [Serializable]
    public class BowView
    {
        private const float DefaultBlendShapeValue = 30f;
        private const float MaxBlendShapeValue = 100f;

        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        [SerializeField] private BowstringAnimation _animation;

        public void PullBowstring(float tension)
        {
            float blendShapeValue = Mathf.Lerp(DefaultBlendShapeValue, MaxBlendShapeValue, tension);
            _skinnedMeshRenderer.SetBlendShapeWeight(0, blendShapeValue);
        }

        public void ReleaseBowstring()
        {
            _animation.PlayReleaseBowstringAnimation(_skinnedMeshRenderer, DefaultBlendShapeValue);
        }
    }
}
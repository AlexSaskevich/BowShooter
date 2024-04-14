using System;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow
{
    [Serializable]
    public class BowView
    {
        [SerializeField] private BowstringAnimation _animation;
        [SerializeField] private Transform _bowstring;
        [SerializeField] private float _defaultY;
        [SerializeField] private float _maxY;

        public void PullBowstring(float tension)
        {
            float targetValue = Mathf.Lerp(_defaultY, _maxY, tension);
            Vector3 bowstringLocalPosition = _bowstring.localPosition;
            bowstringLocalPosition.y = targetValue;
            _bowstring.localPosition = bowstringLocalPosition;
        }

        public void ReleaseBowstring()
        {
            _animation.PlayReleaseBowstringAnimation(_bowstring, _defaultY);
        }
    }
}
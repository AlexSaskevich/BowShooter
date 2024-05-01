using UnityEngine;

namespace Source.Scripts.Utils.Extensions
{
    public static class TransformExtensions
    {
        public static Transform[] GetAllChildTransforms(this Transform parent)
        {
            int childCount = parent.childCount;
            Transform[] childTransforms = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                childTransforms[i] = parent.GetChild(i);
            }

            return childTransforms;
        }
    }
}
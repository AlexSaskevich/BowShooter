using UnityEngine;

namespace Source.Scripts.Utils
{
    public class DrawForward : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Transform targetTransform = transform;
            Gizmos.DrawRay(targetTransform.position, targetTransform.forward);
        }
    }
}

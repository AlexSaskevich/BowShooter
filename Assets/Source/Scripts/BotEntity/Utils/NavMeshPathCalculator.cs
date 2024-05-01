using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Source.Scripts.BotEntity.Utils
{
    public class NavMeshPathCalculator
    {
        private const int MaxDistanceToCheckSamplePosition = 21;

        public Vector3 GetDirectionToNextPoint(Vector3 sourcePosition, Vector3 targetPosition)
        {
            NavMeshPath navMeshPath = new();

            Vector3 enemyPosition = GetSamplePosition(sourcePosition);

            bool canCalculatePath = NavMesh.CalculatePath(enemyPosition, GetSamplePosition(targetPosition),
                NavMesh.AllAreas, navMeshPath);
            
            if (canCalculatePath)
            {
                Vector3 nextPoint = navMeshPath.corners.Skip(1).FirstOrDefault();
                Vector3 directionMove = nextPoint - enemyPosition;

                directionMove.y = 0;
#if UNITY_EDITOR
                for (int i = 0; i < navMeshPath.corners.Length - 1; i++)
                {
                    Debug.DrawLine(navMeshPath.corners[i], navMeshPath.corners[i + 1], Color.red);
                }
#endif
                return Vector3.ClampMagnitude(directionMove, 1f);
            }

            return Vector3.zero;
        }

        public Vector3 GetSamplePosition(Vector3 sourcePosition)
        {
            NavMesh.SamplePosition(sourcePosition, out NavMeshHit hit, MaxDistanceToCheckSamplePosition,
                NavMesh.AllAreas);
            return hit.position;
        }
    }
}
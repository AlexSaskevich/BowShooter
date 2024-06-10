using System.Linq;
using Sirenix.OdinInspector;
using Source.Scripts.Utils.Extensions;
using UnityEditor;
using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Actions.Patrolling
{
    public class PatrolPointsHelper : MonoBehaviour
    {
        [SerializeField] private PatrolPoint[] _patrolPoints;
        [SerializeField] private bool _isNeedShowPointNames = true;
        [SerializeField] private Transform _patrolPointsParent;

        public PatrolPoint[] GetPatrolPoints()
        {
            return _patrolPoints;
        }
#if UNITY_EDITOR

        [Button]
        private void SetPatrolPoints()
        {
            Transform[] allChildTransforms = _patrolPointsParent.GetAllChildTransforms();
            _patrolPoints = new PatrolPoint[allChildTransforms.Length];

            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                PatrolPoint patrolPoint = _patrolPoints[i];
                patrolPoint.WaitTime = 1f;
                patrolPoint.Point = allChildTransforms[i];
                _patrolPoints[i] = patrolPoint;
            }

            EditorUtility.SetDirty(this);
        }

        private static Transform[] GetAllChildTransforms(GameObject parent)
        {
            Transform parentTransform = parent.transform;
            int childCount = parentTransform.childCount;
            Transform[] childTransforms = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                childTransforms[i] = parentTransform.GetChild(i);
            }

            return childTransforms;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (_patrolPoints == null || _patrolPoints.Length == 0)
            {
                return;
            }

            Vector3 previousPoint = _patrolPoints.First().Point.position;

            for (int i = 0; i < _patrolPoints.Length; i++)
            {
                Gizmos.DrawLine(previousPoint, _patrolPoints[i].Point.position);
                previousPoint = _patrolPoints[i].Point.position;

                if (_isNeedShowPointNames == false)
                {
                    continue;
                }

                GUIStyle style = new GUIStyle();
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleCenter;
                Handles.Label(_patrolPoints[i].Point.position, _patrolPoints[i].Point.gameObject.name, style);
            }

            if (_patrolPoints.Length > 1)
            {
                Gizmos.DrawLine(previousPoint, _patrolPoints.First().Point.position);
            }
        }
#endif
    }
}
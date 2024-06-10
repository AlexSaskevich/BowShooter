using System.Linq;
using BehaviorDesigner.Runtime.Tasks;
using Sirenix.OdinInspector;
using Source.Scripts.BotEntity.Infrastructure.Actions;
using Source.Scripts.BotEntity.Infrastructure.Conditions;
using UnityEngine;

namespace Source.Scripts.Utils
{
    public class LinkXMLHelper : MonoBehaviour
    {
        [Button]
        private void ShowAssemblyFullname()
        {
            Debug.Log(typeof(ParallelComplete).Assembly.FullName.Split(',').First());
            Debug.Log(typeof(PerformingActionsSequence).Assembly.FullName.Split(',').First());
            Debug.Log(typeof(PerformingConditions).Assembly.FullName.Split(',').First());
        }

        [Button]
        private void ShowTypeFullName()
        {
            Debug.Log(typeof(ParallelComplete).FullName);
            Debug.Log(typeof(PerformingActionsSequence).FullName);
            Debug.Log(typeof(PerformingConditions).FullName);
        }
    }
}
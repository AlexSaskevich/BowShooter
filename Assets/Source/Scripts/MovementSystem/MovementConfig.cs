using Source.Scripts.Infrastructure;
using UnityEngine;

namespace Source.Scripts.MovementSystem
{
    [CreateAssetMenu(menuName = Constants.Configs + Constants.Movement, fileName = nameof(MovementConfig), order = 0)]
    public class MovementConfig : ScriptableObject
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 7;
    }
}
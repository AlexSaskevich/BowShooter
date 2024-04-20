using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Components
{
    public class Rotation
    {
        private readonly Transform _movable;
        private readonly float _speed;

        public Rotation(Transform movable, float speed = 5f)
        {
            _speed = speed;
            _movable = movable;
        }

        public void Perform(Vector3 direction)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _movable.rotation = Quaternion.Slerp(_movable.rotation, targetRotation, Time.deltaTime * _speed);
        }
    }
}
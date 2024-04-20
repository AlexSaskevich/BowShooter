using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Components
{
    public class Movement
    {
        private readonly CharacterController _characterController;

        public Movement(CharacterController characterController, float speed = 5f)
        {
            _characterController = characterController;
            Speed = speed;
        }

        public float Speed { get; private set; }
        public float NormalizedSpeed => _characterController.velocity.magnitude / Speed;

        public void Perform(Vector3 direction)
        {
            direction *= Speed;
            _characterController.SimpleMove(direction);
        }
    }
}
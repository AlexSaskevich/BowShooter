using UnityEngine;

namespace Source.Scripts.BotEntity.Bots.Components
{
    public class Movement
    {
        public Movement(CharacterController characterController, float speed = 5f)
        {
            CharacterController = characterController;
            Speed = speed;
        }

        public CharacterController CharacterController { get; }
        public float Speed { get; private set; }
        public float NormalizedSpeed => CharacterController.velocity.magnitude / Speed;

        public void Perform(Vector3 direction)
        {
            direction *= Speed;
            CharacterController.SimpleMove(direction);
        }
    }
}
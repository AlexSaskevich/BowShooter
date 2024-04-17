using UnityEngine;

namespace Source.Scripts.BotLogic.Bots.Components
{
    public class Movement
    {
        private readonly CharacterController _characterController;
        private readonly float _speed;

        public Movement(CharacterController characterController, float speed = 5f)
        {
            _characterController = characterController;
            _speed = speed;
        }

        public void Perform(Vector3 direction)
        {
            direction *= _speed;
            _characterController.SimpleMove(direction);
        }
    }
}
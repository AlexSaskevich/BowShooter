using Source.Scripts.Weapon.Projectiles;
using Source.Scripts.Weapon.Projectiles.Arrows;
using UnityEngine;

namespace Source.Scripts.Weapon.Bow.Components
{
    public class Shooting
    {
        private readonly Camera _camera;
        private readonly Vector3 _screenCenter;

        public Shooting(Camera camera)
        {
            _camera = camera;
            _screenCenter = new Vector3(0.5f, 0.5f, 0);
        }

        public void Perform(Arrow arrow, float currentTension)
        {
            Vector3 direction = CalculateDirection(arrow);
            float velocity = ((ArrowConfig)arrow.Config).Speed * currentTension;
            arrow.Fly(direction.normalized, velocity);
        }

        private Vector3 CalculateDirection(Arrow arrow)
        {
            Ray ray = _camera.ViewportPointToRay(_screenCenter);

            Vector3 targetPosition =
                Physics.Raycast(ray, out RaycastHit raycastHit) ? raycastHit.point : ray.GetPoint(100);

            return targetPosition - arrow.ArrowTransform.position;
        }
    }
}
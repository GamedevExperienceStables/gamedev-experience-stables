using Game.Utils;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Projectile/Trajectory/Ballistic")]
    public class ProjectileTrajectoryBallistic : ProjectileTrajectory
    {
        private const float CORRECTION_OFFSET = 0.4f;

        [SerializeField, Range(0f, 90f)]
        private float angle = 45f;

        public override void GetInitialVelocity(Vector3 startPosition, Vector3 targetPosition,
            float speed, out Vector3 startVelocity, out float gravity)
        {
            DebugExtensions.DebugPoint(targetPosition, duration: 1f, color: Color.red);


            targetPosition = Correction(startPosition, targetPosition);

            gravity = speed;
            bool solved = Ballistics.SolveBallisticArc(startPosition, targetPosition, angle, speed, out startVelocity);
            if (!solved)
                startVelocity = Ballistics.GetLinearVelocity(startPosition, targetPosition, speed);

            DebugExtensions.DebugArrow(startPosition, startVelocity, Color.cyan, 1f);
        }

        private static Vector3 Correction(Vector3 startPosition, Vector3 targetPosition)
        {
            if (targetPosition.y < startPosition.y) 
                targetPosition += new Vector3(0, CORRECTION_OFFSET, 0);

            return targetPosition;
        }

        public override void Tick(float deltaTime, float gravityIn, ref Vector3 position, ref Vector3 velocity)
        {
            Vector3 gravityForce = new(0, -gravityIn, 0);

            position += velocity * deltaTime + gravityForce * (0.5f * deltaTime * deltaTime);
            velocity += gravityForce * deltaTime;
        }
    }
}
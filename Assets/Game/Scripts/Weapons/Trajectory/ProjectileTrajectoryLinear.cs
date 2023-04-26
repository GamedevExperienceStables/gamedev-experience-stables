using Game.Utils;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Projectile/Trajectory/Linear")]
    public class ProjectileTrajectoryLinear : ProjectileTrajectory
    {
        public override void GetInitialVelocity(Vector3 startPosition, Vector3 targetPosition,
            float speed, out Vector3 startVelocity, out float gravity)
        {
            DebugExtensions.DebugPoint(targetPosition, duration: 1f, color: Color.red);

            startVelocity = Ballistics.GetLinearVelocity(startPosition, targetPosition, speed);
            gravity = 0;

            DebugExtensions.DebugArrow(startPosition, startVelocity, Color.cyan, 1f);
        }

        public override void Tick(float deltaTime, float gravity, ref Vector3 position, ref Vector3 velocity)
            => position += velocity * deltaTime;
    }
}
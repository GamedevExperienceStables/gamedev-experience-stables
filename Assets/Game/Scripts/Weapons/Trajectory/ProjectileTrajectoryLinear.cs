using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Projectile/Trajectory/Linear")]
    public class ProjectileTrajectoryLinear : ProjectileTrajectory
    {
        [SerializeField, Range(0, 90)]
        private float angle;

        [SerializeField]
        private float mass;

        public override Vector3 GetInitialDirection(Transform transform)
        {
            Quaternion rotation = Quaternion.AngleAxis(-angle, transform.right);
            Vector3 direction = rotation * transform.forward;
            return direction;
        }

        public override Vector3 CalculateVelocity(Vector3 velocity)
        {
            Vector3 gravity = Vector3.down * mass;
            Vector3 calculatedVelocity = velocity + gravity;
            return calculatedVelocity;
        }
    }
}
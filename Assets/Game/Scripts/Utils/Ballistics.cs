using UnityEngine;

namespace Game.Utils
{
    public static class Ballistics
    {
        public static Vector3 GetLinearVelocity(Vector3 startPosition, Vector3 targetPosition, float speed)
            => (targetPosition - startPosition).normalized * speed;

        public static bool SolveBallisticArc(Vector3 position, Vector3 target, float firingAngle,
            float gravity, out Vector3 initialVelocity)
        {
            initialVelocity = default;

            Vector3 direction = target - position;
            float heightDifference = direction.y;

            var diffXZ = new Vector3(direction.x, 0f, direction.z);
            float horizontalDistance = diffXZ.magnitude;

            bool hasSolution = horizontalDistance > 0;
            if (!hasSolution)
                return false;

            float angleInRadians = firingAngle * Mathf.Deg2Rad;
            direction.y = horizontalDistance * Mathf.Tan(angleInRadians);
            horizontalDistance += heightDifference / Mathf.Tan(angleInRadians);

            float speed = Mathf.Sqrt(horizontalDistance * gravity / Mathf.Sin(2 * angleInRadians));
            initialVelocity = speed * direction.normalized;

            return true;
        }
    }
}
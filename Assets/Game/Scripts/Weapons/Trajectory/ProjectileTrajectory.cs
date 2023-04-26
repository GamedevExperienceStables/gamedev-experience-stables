using UnityEngine;

namespace Game.Weapons
{
    public abstract class ProjectileTrajectory : ScriptableObject
    {
        public abstract void GetInitialVelocity(Vector3 startPosition, Vector3 targetPosition, float speed,
            out Vector3 startVelocity, out float gravity);

        public abstract void Tick(float deltaTime, float gravity, ref Vector3 position, ref Vector3 velocity);
    }
}
using UnityEngine;

namespace Game.Weapons
{
    public abstract class ProjectileTrajectory : ScriptableObject
    {
        public abstract Vector3 GetInitialDirection(Transform transform);
        public abstract Vector3 CalculateVelocity(Vector3 velocity);
    }
}
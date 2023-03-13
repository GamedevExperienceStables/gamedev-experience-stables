using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public interface IProjectileSettings
    {
        ProjectileTrajectory Trajectory { get; }
        IEnumerable<DamageDefinition> Damages { get; }
        float Speed { get; }
        ProjectileLifetime LifeTime { get; }
        LayerMask CollisionLayers { get; }
    }
}
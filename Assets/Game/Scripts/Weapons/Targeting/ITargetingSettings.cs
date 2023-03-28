using UnityEngine;

namespace Game.Weapons
{
    public interface ITargetingSettings
    {
        Vector3 TargetPositionOffset { get; }
        bool RelativeToGround { get; }
        bool AllowTargetAbove { get; }
        bool AllowTargetBelow { get; }
        float MinDistanceToTargetBelow { get; }
        float MinDistanceToTarget { get; }
    }
}
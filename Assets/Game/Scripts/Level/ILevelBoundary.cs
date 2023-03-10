using UnityEngine;

namespace Game.Level
{
    public interface ILevelBoundary
    {
        Vector3 Center { get; }
        Vector3 Size { get; }
    }
}
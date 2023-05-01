using UnityEngine;

namespace Game.Level
{
    public interface ILocationBounds
    {
        Vector3 Center { get; }
        Vector3 Size { get; }
        Bounds Bounds { get; }
    }
}
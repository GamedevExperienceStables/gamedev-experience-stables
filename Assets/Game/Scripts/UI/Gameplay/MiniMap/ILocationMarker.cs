using UnityEngine;

namespace Game.UI
{
    public interface ILocationMarker
    {
        Vector3 Position { get; }
        Sprite Icon { get; }
    }
}
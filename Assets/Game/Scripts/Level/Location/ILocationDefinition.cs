using UnityEngine;

namespace Game.Level
{
    public interface ILocationDefinition
    {
        string SceneName { get; }
        Sprite MapImage { get; }
    }
}
using UnityEngine;

namespace Game.Level
{
    public interface ILocationDefinition
    {
        public string Id { get; }
        string SceneName { get; }
        Sprite MapImage { get; }
    }
}
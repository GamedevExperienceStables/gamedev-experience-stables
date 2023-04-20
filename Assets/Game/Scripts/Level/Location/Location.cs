using UnityEngine;

namespace Game.Level
{
    public class Location : ILocationDefinition
    {
        public Location(string sceneName, string id)
        {
            SceneName = sceneName;
            Id = id;
        }

        public string Id { get; }
        public string SceneName { get; }
        public Sprite MapImage => default;
    }
}
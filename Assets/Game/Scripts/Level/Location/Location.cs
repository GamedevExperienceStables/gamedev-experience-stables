namespace Game.Level
{
    public class Location : ILocationDefinition
    {
        public Location(string sceneName) 
            => SceneName = sceneName;

        public string SceneName { get; }
    }
}
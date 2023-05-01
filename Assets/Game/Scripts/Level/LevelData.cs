using Game.Settings;
using JetBrains.Annotations;

namespace Game.Level
{
    [UsedImplicitly]
    public class LevelData
    {
        public LevelDefinition CurrentLevel { get; set; }
        
        public ILocationPoint LastLocationPoint { get; set; }
        public ILocationPoint CurrentLocationPoint { get; set; }

        public LevelLocations Locations { get; set; }
    }
}
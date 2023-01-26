using Game.Settings;
using JetBrains.Annotations;

namespace Game.Level
{
    [UsedImplicitly]
    public class LevelData
    {
        public LevelDefinition CurrentLevel { get; set; }
        
        public LocationPointData LastLocation { get; set; }
        public LocationPointData CurrentLocation { get; set; }

        // список собранных материалов на планете
        
        // список уничтоженных врагов в локациях
        
    }
}
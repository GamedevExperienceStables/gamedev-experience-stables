using Game.Level;
using JetBrains.Annotations;

namespace Game.GameFlow
{
    [UsedImplicitly]
    public class GameStateModel
    {
        public LocationPointDefinition LastLocation { get; set; }
        public LocationPointDefinition CurrentLocation { get; set; }
    }
}
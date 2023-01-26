using System.Collections.Generic;

namespace Game.Persistence
{
    public struct GameSaveData
    {
        public MetaSaveData meta;
        public LevelSaveData level;
        public PlayerSaveData player;

        public struct MetaSaveData
        {
            public string timestampString;
            public double playTime;
        }

        public struct LevelSaveData
        {
            public string id;
            public int materialCollected;
            public List<string> pointsCleared;
        }
        
        public readonly struct PlayerSaveData
        {
            public readonly List<string> runes;
        }
    }
}
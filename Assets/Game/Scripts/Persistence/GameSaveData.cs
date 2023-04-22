namespace Game.Persistence
{
    public struct GameSaveData
    {
        public Meta meta;
        public Level level;
        public Player player;

        public struct Meta
        {
            public string timestampString;
            public uint playTime;
        }

        public struct Level
        {
            public string id;
            public Location[] locations;
        }

        public struct Location
        {
            public string id;
            public LocationCounter[] counters;
        }

        public struct LocationCounter
        {
            public string id;
            public int value;
        }

        public struct Player
        {
            public int containerMaterials;
            public int bagMaterials;
            public string[] runes;
            public string[] slots;
        }
    }
}
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
            public LocationPoint startPoint;
            public Location[] locations;
        }

        public struct Location
        {
            public string id;
            public string[] facts;
            public LocationCounter[] counters;
            public LocationLoot[] loot;
        }

        public struct LocationCounter
        {
            public string id;
            public int value;
        }

        public struct LocationLoot
        {
            public string type;
            public float[] position;
        }

        public struct LocationPoint
        {
            public string id;
            public string location;
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
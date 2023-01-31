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
            public string[] pointsCleared;
        }

        public struct Player
        {
            public int containerMaterials;
            public int bagMaterials;
            public string[] recipes;
        }
    }
}
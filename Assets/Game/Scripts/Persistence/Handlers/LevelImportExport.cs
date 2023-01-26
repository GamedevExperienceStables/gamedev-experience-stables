using Game.Settings;
using VContainer;

namespace Game.Persistence
{
    public class LevelImportExport
    {
        private readonly LevelDataHandler _level;

        [Inject]
        public LevelImportExport(LevelDataHandler level)
        {
            _level = level;
        }

        public void Reset() 
            => _level.Reset();

        public void Import(GameSaveData.LevelSaveData data)
        {
            LevelDefinition currentLevel = _level.FindLevelById(data.id);
            _level.InitLevel(currentLevel);
        }

        public GameSaveData.LevelSaveData Export()
        {
            return new GameSaveData.LevelSaveData
            {
                id = _level.GetCurrentLevelId(),
            };
        }
    }
}
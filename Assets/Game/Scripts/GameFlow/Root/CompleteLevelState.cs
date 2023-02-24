using System;
using Cysharp.Threading.Tasks;
using Game.Inventory;
using Game.Level;
using Game.SceneManagement;
using Game.Settings;
using VContainer;

namespace Game.GameFlow
{
    public class CompleteLevelState : GameState
    {
        private readonly LevelsSettings _settings;
        private readonly IInventoryItems _inventory;
        private readonly LevelController _level;
        private readonly SceneLoader _loader;

        [Inject]
        public CompleteLevelState(LevelsSettings settings, IInventoryItems inventory, LevelController level, SceneLoader loader)
        {
            _settings = settings;
            _inventory = inventory;
            _level = level;
            _loader = loader;
        }

        protected override async UniTask OnEnter()
        {
            LevelDefinition currentLevel = _level.GetCurrentLevel();
            await PlayExitCutsceneIfExists(currentLevel);

            if (_settings.IsLastLevel(currentLevel))
            {
                await Parent.EnterState<CompleteGameState>();
                return;
            }

            LevelDefinition nextLevel = _settings.GetNextLevel(currentLevel);
            InitNextLevel(nextLevel);

            await Parent.EnterState<PlanetState>();
        }

        private async UniTask PlayExitCutsceneIfExists(LevelDefinition currentLevel)
        {
            if (!currentLevel.TryGetCompletionCutsceneName(out string cutsceneName))
                return;

            await _loader.LoadSceneAsync(cutsceneName);
            await UniTask.Delay(TimeSpan.FromSeconds(2));
        }

        private void InitNextLevel(LevelDefinition nextLevel)
        {
            ILocationPoint levelStartPoint = _settings.LevelStartPoint;
            _level.InitLevel(nextLevel, levelStartPoint);
            _inventory.ClearBag();
        }
    }
}
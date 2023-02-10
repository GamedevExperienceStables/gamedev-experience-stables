using Game.GameFlow;
using Game.Inventory;
using Game.Level;
using Game.Player;
using Game.Stats;
using VContainer;

namespace Game.UI
{
    public class GameplayViewModel
    {
        private readonly RootStateMachine _rootStateMachine;
        private readonly GameplayPause _pause;
        private readonly PlayerController _player;
        private readonly InventoryController _inventory;
        private readonly LevelController _level;

        [Inject]
        public GameplayViewModel(
            RootStateMachine rootStateMachine,
            GameplayPause pause,
            PlayerController player,
            InventoryController inventory,
            LevelController level
        )
        {
            _rootStateMachine = rootStateMachine;
            _pause = pause;

            _player = player;
            _inventory = inventory;
            _level = level;
        }

        public void PauseGame()
            => _pause.Enable();

        public void ResumeGame()
            => _pause.Disable();

        public void GoToMainMenu()
            => _rootStateMachine.EnterState<MainMenuState>();

        public void RestartGame()
            => _rootStateMachine.EnterState<LoadGameState>();

        public void HeroStatSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _player.HeroStatSubscribe(key, callback);

        public void HeroStatUnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _player.HeroStatUnSubscribe(key, callback);

        public void LevelBagMaterialSubscribe(MaterialContainer.MaterialChangedEvent callback)
            => _inventory.Materials.Bag.Subscribe(callback);

        public void LevelBagMaterialUnSubscribe(MaterialContainer.MaterialChangedEvent callback)
            => _inventory.Materials.Bag.UnSubscribe(callback);

        public IReadOnlyMaterialData GetCurrentMaterial()
        {
            MaterialDefinition material = _level.GetCurrentLevelGoalMaterial();
            return _inventory.Materials.Bag.GetMaterialData(material);
        }
    }
}
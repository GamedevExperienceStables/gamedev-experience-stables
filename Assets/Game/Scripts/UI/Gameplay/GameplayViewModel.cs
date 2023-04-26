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
        private readonly GameplayPause _pauseSwitcher;
        private readonly PlayerController _player;
        private readonly IInventoryItems _inventory;
        private readonly LevelController _level;

        [Inject]
        public GameplayViewModel(
            RootStateMachine rootStateMachine,
            GameplayPause pauseSwitcher,
            PlayerController player,
            IInventoryItems inventory,
            LevelController level
        )
        {
            _rootStateMachine = rootStateMachine;
            _pauseSwitcher = pauseSwitcher;

            _player = player;
            _inventory = inventory;
            _level = level;
        }

        public void PauseGame()
            => _pauseSwitcher.Enable();

        public void ResumeGame()
            => _pauseSwitcher.Disable();

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
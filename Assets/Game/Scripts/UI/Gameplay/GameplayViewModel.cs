using Game.GameFlow;
using Game.Inventory;
using Game.Player;
using Game.Stats;
using VContainer;

namespace Game.UI
{
    public class GameplayViewModel
    {
        private readonly RootStateMachine _rootStateMachine;
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly PlayerController _player;
        private readonly InventoryController _inventory;

        [Inject]
        public GameplayViewModel(
            RootStateMachine rootStateMachine,
            PlanetStateMachine planetStateMachine,
            PlayerController player,
            InventoryController inventory
        )
        {
            _planetStateMachine = planetStateMachine;
            _rootStateMachine = rootStateMachine;
            
            _player = player;
            _inventory = inventory;
        }

        public void PauseGame() => _planetStateMachine.PushState<PlanetPauseState>();
        public void ResumeGame() => _planetStateMachine.PopState();

        public void GoToMainMenu() => _rootStateMachine.EnterState<MainMenuState>();

        public void HeroStatSubscribe(CharacterStats key, IStats.StatChangedEvent callback) 
            => _player.HeroStatSubscribe(key, callback);

        public void HeroStatUnSubscribe(CharacterStats key, IStats.StatChangedEvent callback) 
            => _player.HeroStatUnSubscribe(key, callback);

        public void BagMaterialsSubscribe(MaterialContainer.MaterialChangedEvent callback) 
            => _inventory.Materials.Bag.Subscribe(callback);
        public void BagMaterialsUnSubscribe(MaterialContainer.MaterialChangedEvent callback) 
            => _inventory.Materials.Bag.UnSubscribe(callback);
    }
}
using Game.Hero;
using Game.Inventory;
using Game.Player;
using Game.Settings;
using VContainer;

namespace Game.Persistence
{
    public class PlayerImportExport
    {
        private readonly HeroStats.InitialStats _initialStats;
        private readonly PlayerController _player;
        private readonly InventoryController _inventory;

        [Inject]
        public PlayerImportExport(HeroStats.InitialStats initialStats, PlayerController player, InventoryController inventory)
        {
            _initialStats = initialStats;
            _player = player;
            _inventory = inventory;
        }

        public void Reset()
        {
            _player.Init(_initialStats);
            _inventory.Init();
        }

        public GameSaveData.PlayerSaveData Export()
        {
            return new GameSaveData.PlayerSaveData();
        }

        public void Import(GameSaveData.PlayerSaveData data)
        {
            _player.Init(_initialStats);
            _inventory.Init();
        }
    }
}
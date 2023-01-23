using Game.Hero;
using VContainer;

namespace Game.Persistence
{
    public class PlayerDataHandler
    {
        private readonly HeroStats.Settings _stats;
        private readonly PlayerData _player;

        [Inject]
        public PlayerDataHandler(HeroStats.Settings stats, PlayerData player)
        {
            _stats = stats;
            _player = player;
        }

        public void Reset()
        {
            _player.Stats.InitStats(_stats);
        }

        public GameSaveData.PlayerSaveData Export()
        {
            return new GameSaveData.PlayerSaveData();
        }

        public void Import(GameSaveData.PlayerSaveData saveData)
        {
            _player.Stats.InitStats(_stats);
            
            // load  
        }
    }
}
using Game.Hero;
using Game.Stats;
using VContainer;

namespace Game.Player
{
    public class PlayerController
    {
        private readonly HeroStats _heroStats;

        [Inject]
        public PlayerController(PlayerData data)
            => _heroStats = data.HeroStats;

        public HeroStats GetStats()
            => _heroStats;

        public void Init(HeroStats.InitialStats initial)
            => _heroStats.Init(initial);

        public void HeroStatSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.Subscribe(key, callback);

        public void HeroStatUnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.UnSubscribe(key, callback);
    }
}
using Game.Player;
using VContainer.Unity;

namespace Game.GameFlow
{
    public class GameEntryPoint : IStartable
    {
        private readonly PlayerGamePrefs _settings;

        public GameEntryPoint(PlayerGamePrefs settings)
            => _settings = settings;

        public void Start()
            => _settings.Init();
    }
}
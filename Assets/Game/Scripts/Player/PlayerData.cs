using Game.Hero;
using JetBrains.Annotations;

namespace Game.Player
{
    [UsedImplicitly]
    public class PlayerData
    {
        public HeroStats HeroStats { get; } = new();
    }
}
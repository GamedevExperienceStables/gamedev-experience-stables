namespace Game.Stats
{
    public interface IMovableStats : IStatsSet
    {
        CharacterStat Movement { get; }
    }
}
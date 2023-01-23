namespace Game.Stats
{
    public interface IDamageableStats : IStatsSet
    {
        public CharacterStatWithMax Health { get; }
    }
}
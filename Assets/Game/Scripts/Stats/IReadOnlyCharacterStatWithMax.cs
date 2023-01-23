namespace Game.Stats
{
    public interface IReadOnlyCharacterStatWithMax
    {
        IReadOnlyCharacterStat Current { get; }
        IReadOnlyCharacterStat Max { get; }
    }
}
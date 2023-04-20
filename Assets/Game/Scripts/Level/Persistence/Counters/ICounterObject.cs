namespace Game.Level
{
    public interface ICounterObject
    {
        bool IsDirty { get; }
        
        int RemainingCount { get; }
        
        void SetCount(int count);
    }
}
namespace Game.Level
{
    public interface ICounterObject
    {
        bool IsDirty { get; set; }

        int Count { get; }
        
        void SetCount(int count);
    }
}
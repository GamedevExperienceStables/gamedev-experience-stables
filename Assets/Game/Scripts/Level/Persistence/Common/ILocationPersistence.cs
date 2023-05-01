namespace Game.Level
{
    public interface ILocationPersistence<T>
    {
        string Id { get; }

        bool IsDirty { get; }

        T Value { get; set; }
    }
}
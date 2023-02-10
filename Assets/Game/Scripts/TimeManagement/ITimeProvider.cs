namespace Game.TimeManagement
{
    public interface ITimeProvider
    {
        float WorldTime { get; }
        float RealtimeSinceStartup  { get; }
    }
}
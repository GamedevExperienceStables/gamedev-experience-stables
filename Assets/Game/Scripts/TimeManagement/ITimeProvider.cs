namespace Game.TimeManagement
{
    public interface ITimeProvider
    {
        float WorldTime { get; }
        float UnscaledTime { get; }
        float RealtimeSinceStartup  { get; }
        float DeltaTime { get; }
    }
}
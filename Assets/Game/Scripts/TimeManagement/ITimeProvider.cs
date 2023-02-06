namespace Game.TimeManagement
{
    public interface ITimeProvider
    {
        float WorldTime { get; }
        float UnscaledTime { get; }
        float DeltaTime { get; }
    }
}
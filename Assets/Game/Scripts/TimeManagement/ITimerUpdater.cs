namespace Game.TimeManagement
{
    public interface ITimerUpdater
    {
        void Add(TimerUpdatable timer);
        void Remove(TimerUpdatable timer);
    }
}
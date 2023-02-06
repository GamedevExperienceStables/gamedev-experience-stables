using VContainer;

namespace Game.TimeManagement
{
    public class TimeService : ITimeProvider, ITimeService
    {
        [Inject]
        public TimeService()
        {
        }

        public void Pause() => UnityEngine.Time.timeScale = 0;
        public void Play() => UnityEngine.Time.timeScale = 1;

        public float WorldTime => UnityEngine.Time.time;
        public float UnscaledTime => UnityEngine.Time.unscaledTime;
        public float DeltaTime => UnityEngine.Time.deltaTime;
    }
}
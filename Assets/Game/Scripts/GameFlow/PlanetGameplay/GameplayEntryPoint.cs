using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public class GameplayEntryPoint : IStartable
    {
        private readonly PlanetStateMachine _planetStateMachine;

        [Inject]
        public GameplayEntryPoint(PlanetStateMachine planetStateMachine)
        {
            _planetStateMachine = planetStateMachine;
        }

        public void Start()
        {
            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }
    }
}
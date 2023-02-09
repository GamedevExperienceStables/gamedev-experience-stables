using System;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public sealed class GameplayEntryPoint : IStartable, IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayInputTracker _inputTracker;

        [Inject]
        public GameplayEntryPoint(PlanetStateMachine planetStateMachine, GameplayInputTracker inputTracker)
        {
            _planetStateMachine = planetStateMachine;
            _inputTracker = inputTracker;
        }

        public void Start()
        {
            _inputTracker.Start();
            
            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }

        public void Dispose() 
            => _inputTracker.Dispose();
    }
}
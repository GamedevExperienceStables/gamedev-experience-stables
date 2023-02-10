using System;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public sealed class GameplayEntryPoint : IStartable, IDisposable
    {
        private readonly PlanetStateMachine _planetStateMachine;
        private readonly GameplayGameOver _gameOver;

        [Inject]
        public GameplayEntryPoint(PlanetStateMachine planetStateMachine, GameplayGameOver gameOver)
        {
            _planetStateMachine = planetStateMachine;
            _gameOver = gameOver;
        }

        public void Start()
        {
            _gameOver.Start();

            _planetStateMachine.EnterState<PlanetLocationLoadingState>();
        }

        public void Dispose()
            => _gameOver.Dispose();
    }
}
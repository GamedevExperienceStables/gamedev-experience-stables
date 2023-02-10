using System;
using Game.Hero;
using Game.Player;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayGameOver : IDisposable
    {
        private readonly PlayerController _player;
        private readonly PlanetStateMachine _planetStateMachine;
        
        [Inject]
        public GameplayGameOver(PlayerController player, PlanetStateMachine planetStateMachine)
        {
            _player = player;
            _planetStateMachine = planetStateMachine;
        }

        public void Start()
            => _player.HeroDiedSubscribe(OnHeroDied);

        public void Dispose()
            => _player.HeroDiedUnSubscribe(OnHeroDied);

        private void OnHeroDied(HeroController hero)
        {
            hero.RemoveAbilities();
            _planetStateMachine.PushState<PlanetGameOverState>();
        }
    }
}
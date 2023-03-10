using System;
using Game.Actors;
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
            hero.CancelEffects();
            
            if (CanRevive(hero))
                return;

            hero.RemoveAbilities();
            _planetStateMachine.EnterState<PlanetGameOverState>();
        }

        private static bool CanRevive(IActorController hero)
        {
            var reviveAbility = hero.GetAbility<ReviveAbility>();
            return reviveAbility.TryActivateAbility();
        }
    }
}
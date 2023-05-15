using System;
using Game.Actors;
using Game.Actors.Health;
using Game.Hero;
using Game.Inventory;
using Game.Player;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow
{
    public sealed class GameplayGameOver : IStartable, IDisposable
    {
        private readonly PlayerController _player;
        private readonly InventoryController _inventory;
        private readonly PlanetStateMachine _planetStateMachine;

        [Inject]
        public GameplayGameOver(PlayerController player, InventoryController inventory,
            PlanetStateMachine planetStateMachine)
        {
            _player = player;
            _inventory = inventory;
            _planetStateMachine = planetStateMachine;
        }

        public void Start()
            => _player.HeroDiedSubscribe(OnHeroDied);

        public void Dispose()
            => _player.HeroDiedUnSubscribe(OnHeroDied);

        private void OnHeroDied(DeathCause deathCause)
        {
            HeroController hero = _player.Hero;
            hero.CancelEffects();

            if (deathCause is DeathCause.Damage && CanRevive(hero))
                return;

            GameOver(hero);
        }

        private void GameOver(ActorController hero)
        {
            hero.RemoveAbilities();
            _planetStateMachine.EnterState<PlanetGameOverState>();
        }

        private bool CanRevive(IActorController hero)
        {
            var reviveAbility = hero.GetAbility<ReviveAbility>();
            bool isRevived = reviveAbility.TryActivateAbility();
            if (isRevived)
                RemoveRune(reviveAbility);

            return isRevived;
        }

        private void RemoveRune(ActorAbility ability)
        {
            var runes = _inventory.Runes.Items;
            for (int i = runes.Count - 1; i >= 0; i--)
            {
                if (runes[i].GrantAbility == ability.Definition)
                    _inventory.RemoveRune(runes[i]);
            }
        }
    }
}
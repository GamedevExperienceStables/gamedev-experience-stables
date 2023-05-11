using System;
using System.Data;
using Game.Actors;
using Game.Actors.Health;
using Game.Hero;
using Game.Stats;
using UnityEngine;
using VContainer;

namespace Game.Player
{
    public sealed class PlayerController
    {
        private readonly HeroStats _heroStats;
        private bool _isBound;

        private event Action<DeathCause> HeroDeathEvent;

        [Inject]
        public PlayerController(PlayerData data)
            => _heroStats = data.HeroStats;

        public HeroController Hero { get; private set; }
        
        public event Action<HeroController> HeroBound;
        public event Action HeroUnBound;

        public void Init(HeroStats.InitialStats initial)
            => _heroStats.Init(initial);

        public void HeroStatSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.Subscribe(key, callback);

        public void HeroStatUnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.UnSubscribe(key, callback);

        public void HeroDiedSubscribe(Action<DeathCause> callback)
            => HeroDeathEvent += callback;

        public void HeroDiedUnSubscribe(Action<DeathCause> callback)
            => HeroDeathEvent -= callback;

        public void BindHero(HeroController hero)
        {
            Hero = hero;
            Hero.Bind(_heroStats);

            _isBound = true;

            SubscribeHeroDeath(Hero);
            
            HeroBound?.Invoke(Hero);
        }

        public void UnbindHero()
        {
            UnSubscribeHeroDeath(Hero);

            _isBound = false;

            Hero = null;
            
            HeroUnBound?.Invoke();
        }

        private void SubscribeHeroDeath(Component hero)
        {
            if (!hero.TryGetComponent(out DeathController death))
                throw new NoNullAllowedException("Trying subscribe on hero death, but DeathController not exist");

            death.DiedWithCause += OnDeath;
        }

        private void UnSubscribeHeroDeath(Component hero)
        {
            if (!hero)
                return;

            if (!hero.TryGetComponent(out DeathController death))
                throw new NoNullAllowedException("Trying unsubscribe on hero death, but DeathController not exist");

            death.DiedWithCause -= OnDeath;
        }

        private void OnDeath(DeathCause deathCause)
            => HeroDeathEvent?.Invoke(deathCause);

        public bool CanActivateAbility(AbilityDefinition runeGrantAbility)
            => _isBound && Hero.CanActivate(runeGrantAbility);
    }
}
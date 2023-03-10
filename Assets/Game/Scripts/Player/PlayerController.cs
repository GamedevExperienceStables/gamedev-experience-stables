using System;
using System.Data;
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

        private DeathController _deathController;
        private HeroController _hero;

        private event Action<HeroController> HeroDeathEvent;

        [Inject]
        public PlayerController(PlayerData data)
            => _heroStats = data.HeroStats;

        public void Init(HeroStats.InitialStats initial)
            => _heroStats.Init(initial);

        public void HeroStatSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.Subscribe(key, callback);

        public void HeroStatUnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => _heroStats.UnSubscribe(key, callback);

        public void HeroDiedSubscribe(Action<HeroController> callback)
            => HeroDeathEvent += callback;

        public void HeroDiedUnSubscribe(Action<HeroController> callback)
            => HeroDeathEvent -= callback;

        public Vector3 GetHeroPosition() 
            => _hero ? _hero.transform.position : Vector3.zero;
        
        public Quaternion GetHeroRotation() 
            => _hero ? _hero.transform.rotation : Quaternion.identity;

        public void BindHero(HeroController hero)
        {
            _hero = hero;
            _hero.Bind(_heroStats);

            if (!_hero.TryGetComponent(out _deathController))
                throw new NoNullAllowedException("Trying subscribe on hero death, but DeathController not exist");

            _deathController.Died += OnDeath;
        }

        public void UnbindHero()
        {
            if (_deathController)
                _deathController.Died -= OnDeath;

            _deathController = null;
            _hero = null;
        }

        private void OnDeath()
            => HeroDeathEvent?.Invoke(_hero);
    }
}
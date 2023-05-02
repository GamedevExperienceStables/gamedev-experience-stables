using System;
using Game.Player;
using Game.Stats;
using Game.TimeManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class HudDamageView
    {
        private readonly PlayerController _player;
        private readonly HudDamageFx _fx;
        private readonly TimerPool _timers;
        private VisualElement _container;
        private TimerUpdatable _timer;

        public HudDamageView(PlayerController player, HudDamageFx fx, TimerPool timers)
        {
            _player = player;
            _fx = fx;
            _timers = timers;
        }

        public void Create(VisualElement root)
        {
            _container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_DAMAGE);

            _player.HeroStatSubscribe(CharacterStats.Health, OnHealthChanged);
            _timer = _timers.GetTimer(TimeSpan.FromMilliseconds(100), RemoveDamage);
        }

        public void Destroy()
        {
            _player.HeroStatUnSubscribe(CharacterStats.Health, OnHealthChanged);
            _timers.ReleaseTimer(_timer);
            _fx.Destroy();
        }

        private void OnHealthChanged(StatValueChange change)
        {
            if (Mathf.Floor(change.newValue) < Mathf.Floor(change.oldValue))
                DamageFeedback();
        }

        private void DamageFeedback()
        {
            _timer.Start();
            _container.AddToClassList(LayoutNames.Hud.WIDGET_DAMAGE_ENABLED_CLASS_NAME);

            _fx.DamageFeedback();
        }

        private void RemoveDamage()
        {
            _container.RemoveFromClassList(LayoutNames.Hud.WIDGET_DAMAGE_ENABLED_CLASS_NAME);
            _timer.Stop();
        }
    }
}
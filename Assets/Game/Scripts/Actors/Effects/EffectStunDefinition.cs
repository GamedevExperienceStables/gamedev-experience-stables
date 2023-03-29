using System;
using Game.TimeManagement;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Stun")]
    public class EffectStunDefinition : EffectDefinition<EffectStun>
    {
        public override bool CanExecute(IActorController target)
            => true;
    }

    public class EffectStun : Effect<EffectStunDefinition>
    {
        private readonly TimerPool _timers;

        private TimerUpdatable _timer;

        private IActorInputController _brain;

        [Inject]
        public EffectStun(TimerPool timers)
            => _timers = timers;

        protected override void OnExecute(IActorController target)
        {
            _brain = target.GetComponent<IActorInputController>();
            _brain.BlockInput(true);

            _timer = _timers.GetTimerStarted(TimeSpan.FromSeconds(Definition.Duration), Cancel);
        }

        protected override void OnCancel()
        {
            _brain.BlockInput(false);
            _timers.ReleaseTimer(_timer);
        }
    }
}
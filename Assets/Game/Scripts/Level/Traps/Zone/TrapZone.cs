using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public class TrapZone
    {
        private readonly TrapZoneEffectBehaviour _zone;
        private readonly EffectHandler _effectHandler;

        public TrapZone(TrapZoneEffectBehaviour zone, EffectHandler effectHandler)
        {
            _zone = zone;
            _effectHandler = effectHandler;
        }

        public LayerMask LayerMask => _zone.LayerMask;

        public void Enter(IActorController target)
        {
            if (_zone.Behaviour is TrapZoneBehaviour.AddOnEnter or TrapZoneBehaviour.AddOnEnterRemoveOnExit)
                _effectHandler.ApplyEffects(target, _zone.Effects, this);
        }

        public void Exit(IActorController target)
        {
            if (_zone.Behaviour is TrapZoneBehaviour.AddOnExit)
                ApplyEffect(target);
            
            if (_zone.Behaviour is TrapZoneBehaviour.AddOnEnterRemoveOnExit)
                target.RemoveEffectsByInstigator(this);
        }

        private void ApplyEffect(IActorController target)
            => _effectHandler.ApplyEffects(target, _zone.Effects, this);
    }
}
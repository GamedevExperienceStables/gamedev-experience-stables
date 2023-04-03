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
            switch (_zone.Behaviour)
            {
                case TrapZoneBehaviour.AddOnExit:
                    _effectHandler.ApplyEffects(target, _zone.Effects, this);
                    break;
                
                case TrapZoneBehaviour.AddOnEnterRemoveOnExit:
                    _effectHandler.CancelEffectsByInstigator(target, _zone.Effects, this);
                    break;
            }
        }
    }
}
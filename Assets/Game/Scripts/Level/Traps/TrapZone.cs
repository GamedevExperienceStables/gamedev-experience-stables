using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public class TrapZone
    {
        private readonly TrapZoneDefinition _definition;
        private readonly EffectHandler _effectHandler;

        public TrapZone(TrapZoneDefinition definition, EffectHandler effectHandler)
        {
            _definition = definition;
            _effectHandler = effectHandler;
        }

        public LayerMask LayerMask => _definition.LayerMask;

        public void Enter(IActorController target) 
            => _effectHandler.ApplyEffects(target, _definition.Effects, this);

        public void Exit(IActorController target)
        {
            if (!_definition.RemoveEffectsOnExit)
                return;

            target.RemoveEffectsByInstigator(this);
        }
    }
}
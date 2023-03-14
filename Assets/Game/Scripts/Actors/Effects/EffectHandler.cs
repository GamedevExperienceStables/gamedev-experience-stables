using System.Collections.Generic;
using VContainer;

namespace Game.Actors
{
    public class EffectHandler 
    {
        private readonly EffectFactory _effectFactory;

        [Inject]
        public EffectHandler(EffectFactory effectFactory) 
            => _effectFactory = effectFactory;

        public void ApplyEffects(IActorController target, IEnumerable<EffectDefinition> effects, object instigator)
        {
            foreach (EffectDefinition definition in effects)
            {
                if (!definition.CanExecute(target)) 
                    continue;
                
                Effect effect = definition.CreateRuntimeInstance(_effectFactory);
                effect.Instigator = instigator;
                target.AddEffect(effect);
            }
        }
    }
}
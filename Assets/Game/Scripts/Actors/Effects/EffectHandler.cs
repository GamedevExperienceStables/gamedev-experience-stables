using System.Collections.Generic;
using Game.Stats;
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
            if (IsDead(target))
                return;

            foreach (EffectDefinition definition in effects)
            {
                if (!definition.CanExecute(target))
                    continue;

                if (definition.EffectType is EffectDuration.Instant)
                    ApplyEffect(target, definition, instigator);
                else
                    AddEffect(target, definition, instigator);
            }
        }

        private void AddEffect(IActorController target, EffectDefinition definition, object instigator)
        {
            Effect effect = CreateEffect(definition, instigator);
            target.AddEffect(effect);
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void CancelEffectsByInstigator(IActorController target,
            IEnumerable<EffectDefinition> effects, object instigator)
            => target.CancelEffectsByInstigator(instigator, effects);

        private void ApplyEffect(IActorController target, EffectDefinition definition, object instigator)
        {
            Effect effect = CreateEffect(definition, instigator);
            target.ApplyEffect(effect);
        }

        private Effect CreateEffect(EffectDefinition definition, object instigator)
        {
            Effect effect = definition.CreateRuntimeInstance(_effectFactory);
            effect.Instigator = instigator;
            return effect;
        }

        private static bool IsDead(IActorController target)
            => target.GetCurrentValue(CharacterStats.Health) == 0;
    }
}
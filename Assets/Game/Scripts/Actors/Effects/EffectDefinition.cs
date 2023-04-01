using Game.Utils.Factory;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Actors
{
    public abstract class EffectDefinition : DefinitionFactory<Effect, EffectFactory>
    {
        protected const string MENU_PATH = "★ Effect/";
        
        [SerializeField]
        private EffectDuration effectType;
        
        [Space]
        [SerializeField, HideIf(nameof(effectType), EffectDuration.Instant)]
        private StatusDefinition status;

        [SerializeField, Min(0)]
        [ShowIf(nameof(effectType), EffectDuration.Limited)]
        private float duration = 3f;

        public StatusDefinition Status => status;
        public float Duration => duration;

        public EffectDuration EffectType => effectType;

        public abstract bool CanExecute(IActorController target);
    }
    
    public abstract class EffectDefinition<T> : EffectDefinition where T : Effect
    {
        public override Effect CreateRuntimeInstance(EffectFactory factory)
            => factory.Create<T>(this);
    }
}
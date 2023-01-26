using VContainer;

namespace Game.Effects
{
    public class GameplayEffectFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public GameplayEffectFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public TEffect Create<TEffect, TDefinition>(TDefinition definition) where TEffect : GameplayEffect<TDefinition>
        {
            var effect = _resolver.Resolve<TEffect>();
            effect.Init(definition);
            return effect;
        }
    }
}
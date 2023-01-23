using System;
using VContainer;

namespace Game.Effects
{
    public class GameplayEffectFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public GameplayEffectFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public GameplayEffect Create(GameplayEffectDefinition definition) =>
            definition switch
            {
                EffectChangeStatDefinition => _resolver.Resolve<EffectChangeStatDefinition.EffectChangeStat>(),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}
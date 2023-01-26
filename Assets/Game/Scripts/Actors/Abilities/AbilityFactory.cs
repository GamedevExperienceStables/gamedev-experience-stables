using VContainer;

namespace Game.Actors
{
    public class AbilityFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public AbilityFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public ActorAbility Create<T>(AbilityDefinition<T> definition) where T : ActorAbility
        {
            var ability = _resolver.Resolve<T>();
            ability.CreateAbility(definition);
            return ability;
        }
    }
}
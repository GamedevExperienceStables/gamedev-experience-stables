using VContainer;

namespace Game.Utils.Factory
{
    public abstract class ScriptableObjectFactory<TDefinition>
    {
        private readonly IObjectResolver _resolver;

        protected ScriptableObjectFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public TInstance Create<TInstance>(TDefinition definition) where TInstance : IRuntimeInstance<TDefinition>
        {
            var effect = _resolver.Resolve<TInstance>();
            effect.OnCreate(definition);
            return effect;
        }
    }
}
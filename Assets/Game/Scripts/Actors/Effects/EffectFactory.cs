using Game.Utils.Factory;
using VContainer;

namespace Game.Actors
{
    public class EffectFactory : ScriptableObjectFactory<EffectDefinition>
    {
        [Inject]
        public EffectFactory(IObjectResolver resolver) : base(resolver)
        {
        }
    }
}
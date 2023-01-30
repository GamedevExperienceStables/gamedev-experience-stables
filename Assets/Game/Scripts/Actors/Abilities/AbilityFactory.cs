using Game.Utils.Factory;
using VContainer;

namespace Game.Actors
{
    public class AbilityFactory : ScriptableObjectFactory<AbilityDefinition>
    {
        [Inject]
        public AbilityFactory(IObjectResolver resolver) : base(resolver)
        {
        }
    }
}
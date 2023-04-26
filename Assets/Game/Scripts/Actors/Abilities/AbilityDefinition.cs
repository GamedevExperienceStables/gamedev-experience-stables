using Game.Utils.Factory;

namespace Game.Actors
{
    public abstract class AbilityDefinition : DefinitionFactory<ActorAbility, AbilityFactory>
    {
        protected const string MENU_PATH = "★ Abilities/";
    }

    public abstract class AbilityDefinition<T> : AbilityDefinition where T : ActorAbility
    {
        public override ActorAbility CreateRuntimeInstance(AbilityFactory factory)
            => factory.Create<T>(this);
    }
}
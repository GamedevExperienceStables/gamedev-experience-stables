using UnityEngine;

namespace Game.Actors
{
    public abstract class AbilityDefinition : ScriptableObject
    {
        protected const string MENU_PATH = "★ Abilities/";
        
        public abstract ActorAbility CreateRuntimeInstance(AbilityFactory factory);
    }

    public abstract class AbilityDefinition<T> : AbilityDefinition where T : ActorAbility
    {
        public override ActorAbility CreateRuntimeInstance(AbilityFactory factory)
            => factory.Create<T>(this);
    }
}
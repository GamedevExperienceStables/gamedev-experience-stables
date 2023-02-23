using System.Collections.Generic;
using UnityEngine;

namespace Game.Actors
{
    public class ActorAbilities
    {
        private readonly Dictionary<AbilityDefinition, ActorAbility> _abilities = new();

        public void RegisterAbility(ActorAbility ability, IActorController owner)
        {
            ability.Owner = owner;

            _abilities.Add(ability.Definition, ability);
        }

        public void GiveAbility(AbilityDefinition definition)
        {
            if (!TryGetAbility(definition, out ActorAbility ability))
            {
                Debug.LogWarning($"Trying give ability that not exits: {definition.name}");
                return;
            }

            ability.GiveAbility();
        }

        public void RemoveAbility(AbilityDefinition definition)
        {
            if (!TryGetAbility(definition, out ActorAbility ability))
            {
                Debug.LogWarning($"Trying remove ability that not exits: {definition.name}");
                return;
            }

            ability.RemoveAbility();
        }

        public T GetAbility<T>() where T : ActorAbility
        {
            foreach (ActorAbility ability in _abilities.Values)
            {
                if (ability.GetType() == typeof(T))
                    return ability as T;
            }

            return default;
        }

        public bool TryGetAbility<T>(out T foundAbility) where T : ActorAbility
        {
            foreach (ActorAbility ability in _abilities.Values)
            {
                if (ability.GetType() == typeof(T))
                {
                    foundAbility = ability as T;
                    return true;
                }
            }

            foundAbility = default;
            return false;
        }

        public bool TryGetAbility(AbilityDefinition definition, out ActorAbility foundAbility)
            => _abilities.TryGetValue(definition, out foundAbility);

        public void InitAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.InitAbility();
        }

        public void ResetAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.ResetAbility();
        }

        public void RemoveAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.RemoveAbility();
        }

        public void DestroyAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.DestroyAbility();
        }
    }
}
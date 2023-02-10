using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Actors
{
    public class ActorAbilities
    {
        private readonly Dictionary<Type, ActorAbility> _abilities = new();

        public void RegisterAbility(ActorAbility ability, IActorController owner)
        {
            ability.Owner = owner;

            _abilities.Add(ability.GetType(), ability);
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
            => _abilities[typeof(T)] as T;

        public bool TryGetAbility<T>(out T foundAbility) where T : ActorAbility
        {
            if (_abilities.TryGetValue(typeof(T), out ActorAbility ability))
            {
                foundAbility = ability as T;
                return true;
            }

            foundAbility = default;
            return false;
        }

        public bool TryGetAbility(AbilityDefinition definition, out ActorAbility foundAbility)
        {
            foreach (ActorAbility ability in _abilities.Values)
            {
                if (!ReferenceEquals(ability.Definition, definition))
                    continue;

                foundAbility = ability;
                return true;
            }

            foundAbility = default;
            return false;
        }

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
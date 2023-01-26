using System;
using System.Collections.Generic;
using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    public abstract class ActorController : MonoBehaviour, IActorController
    {
        public Transform Transform => transform;
        
        public abstract IStatsSet Stats { get; }

        private readonly Dictionary<Type, ActorAbility> _abilities = new();

        private void Awake()
            => OnAwake();

        protected virtual void OnAwake()
        {
        }

        private void OnDestroy()
            => DestroyAbilities();

        public bool HasStats<T>() where T : IStatsSet
            => Stats is T;

        public T GetStats<T>() where T : IStatsSet
            => (T)Stats;

        public void RegisterAbility(ActorAbility ability)
        {
            ability.Owner = this;
            
            _abilities[ability.GetType()] = ability;
        }

        public void GiveAbility(AbilityDefinition definition)
        {
            foreach (ActorAbility ability in _abilities.Values)
            {
                if (ability.InstanceOf(definition))
                    ability.GiveAbility();
            }
        }
        
        public void RemoveAbility(AbilityDefinition definition)
        {
            foreach (ActorAbility ability in _abilities.Values)
            {
                if (ability.InstanceOf(definition))
                    ability.RemoveAbility();
            }
        }

        public T FindAbility<T>() where T : ActorAbility
        {
            if (_abilities.TryGetValue(typeof(T), out ActorAbility foundAbility))
                return (T)foundAbility;

            return null;
        }
        
        public void InitAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values) 
                ability.InitAbility();
        }

        protected void ResetAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.ResetAbility();
        }

        private void DestroyAbilities()
        {
            foreach (ActorAbility ability in _abilities.Values)
                ability.DestroyAbility();
        }
    }
}
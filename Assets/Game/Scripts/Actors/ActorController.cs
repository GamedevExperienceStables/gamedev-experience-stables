using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    public abstract class ActorController : MonoBehaviour, IActorController
    {
        private readonly ActorAbilities _abilities = new();

        public Transform Transform => transform;

        protected abstract IStats Stats { get; }

        private void Awake()
            => OnActorAwake();

        private void OnDestroy()
        {
            DestroyAbilities();

            OnActorDestroy();
        }

        public bool HasStat(CharacterStats key)
            => Stats.HasStat(key);

        public float GetCurrentValue(CharacterStats key)
            => Stats[key];

        public void ApplyModifier(CharacterStats key, StatModifier modifier)
        {
            var data = new StatModifierApplyData(key, modifier.Type, modifier.Value, this);
            Stats.ApplyModifier(key, data);
        }
        
        public void ApplyModifier(CharacterStats key, float value)
        {
            var data = new StatModifierApplyData(key, StatsModifierType.Flat, value, this);
            Stats.ApplyModifier(key, data);
        }

        public void AddModifier(CharacterStats key, StatModifier modifier)
            => Stats.AddModifier(key, modifier);

        public void RemoveModifier(CharacterStats key, StatModifier modifier)
            => Stats.RemoveModifier(key, modifier);

        public void Subscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => Stats.Subscribe(key, callback);

        public void UnSubscribe(CharacterStats key, IStats.StatChangedEvent callback)
            => Stats.UnSubscribe(key, callback);

        public void RegisterAbility(ActorAbility ability)
            => _abilities.RegisterAbility(ability, this);

        public void GiveAbility(AbilityDefinition definition)
            => _abilities.GiveAbility(definition);

        public void RemoveAbility(AbilityDefinition definition)
            => _abilities.RemoveAbility(definition);

        public bool TryGetAbility(AbilityDefinition definition, out ActorAbility foundAbility)
            => _abilities.TryGetAbility(definition, out foundAbility);

        public bool TryGetAbility<T>(out T foundAbility) where T : ActorAbility
            => _abilities.TryGetAbility(out foundAbility);

        public T GetAbility<T>() where T : ActorAbility
            => _abilities.GetAbility<T>();

        public void InitAbilities()
            => _abilities.InitAbilities();

        protected void ResetAbilities()
            => _abilities.ResetAbilities();
        
        public void RemoveAbilities() 
            => _abilities.RemoveAbilities();

        private void DestroyAbilities()
            => _abilities.DestroyAbilities();

        protected virtual void OnActorAwake()
        {
        }

        protected virtual void OnActorDestroy()
        {
        }
    }
}
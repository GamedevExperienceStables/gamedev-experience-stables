using System.Collections.Generic;
using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    public abstract class ActorController : MonoBehaviour, IActorController
    {
        public abstract IStatsSet Stats { get; }

        public IEnumerable<ActorAbilityView> Abilities => _abilities;

        private ActorAbilityView[] _abilities;

        private void Awake()
        {
            OnAwake();
            CollectAbilities();
        }

        protected virtual void OnAwake()
        {
        }

        private void Start()
            => InitAbilities();

        private void OnDestroy()
            => DestroyAbilities();

        public T GetStats<T>() where T : IStatsSet
            => (T)Stats;

        public T FindAbility<T>() where T : ActorAbilityView
        {
            foreach (ActorAbilityView ability in Abilities)
            {
                if (ability is T foundAbility)
                    return foundAbility;
            }

            return default;
        }

        protected void ResetAbilities()
        {
            foreach (ActorAbilityView ability in Abilities)
                ability.ResetAbility();
        }

        private void CollectAbilities()
        {
            _abilities = GetComponents<ActorAbilityView>();
            foreach (ActorAbilityView ability in Abilities)
                ability.Owner = this;
        }

        private void InitAbilities()
        {
            foreach (ActorAbilityView ability in Abilities)
                ability.InitAbility();
        }

        private void DestroyAbilities()
        {
            foreach (ActorAbilityView ability in Abilities)
                ability.DestroyAbility();
        }
    }
}
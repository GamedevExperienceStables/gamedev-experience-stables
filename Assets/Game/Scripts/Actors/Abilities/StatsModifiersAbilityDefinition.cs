using System.Collections.Generic;
using Game.Stats;
using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Stats Modifiers")]
    public class StatsModifiersAbilityDefinition : AbilityDefinition<StatsModifiersAbility>
    {
        [SerializeField]
        private List<CharacterStatModifier> modifiers;

        public List<CharacterStatModifier> Modifiers => modifiers;
    }

    public class StatsModifiersAbility : ActorAbility<StatsModifiersAbilityDefinition>
    {
        public override bool CanActivateAbility()
            => !IsActive;

        protected override void OnGiveAbility() 
            => ActivateAbility();

        protected override void OnActivateAbility()
        {
            foreach (CharacterStatModifier modifier in Definition.Modifiers)
                Owner.AddModifier(modifier.Stat, modifier.Modifier);
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            foreach (CharacterStatModifier modifier in Definition.Modifiers)
                Owner.RemoveModifier(modifier.Stat, modifier.Modifier);
        }
    }
}
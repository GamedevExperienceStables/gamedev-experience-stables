using Game.Actors;
using Game.Stats;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Stone")]
    public class ChangeStatItemDefinition : ConsumableDefinition
    {
        [SerializeField]
        private CharacterStats stat;

        [SerializeField]
        private StatModifier modifier;

        public override bool CanExecute(ItemExecutionContext context)
        {
            IActorController target = context.target;
            if (!target.HasStat(stat))
                return false;

            if (modifier.Value > 0)
                return !IsMax(target);

            return true;
        }

        private bool IsMax(IActorController target) =>
            stat switch
            {
                CharacterStats.Health => target.GetCurrentValue(CharacterStats.Health) >=
                                         target.GetCurrentValue(CharacterStats.HealthMax),
                CharacterStats.Mana => target.GetCurrentValue(CharacterStats.Mana) >=
                                       target.GetCurrentValue(CharacterStats.ManaMax),
                CharacterStats.Stamina => target.GetCurrentValue(CharacterStats.Stamina) >=
                                          target.GetCurrentValue(CharacterStats.StaminaMax),
                _ => false
            };

        public override void Execute(ItemExecutionContext context)
            => context.target.ApplyModifier(stat, modifier);
    }
}
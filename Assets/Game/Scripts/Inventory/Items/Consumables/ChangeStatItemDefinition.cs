using Game.Stats;
using UnityEngine;

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
            => context.target.HasStat(stat);

        public override void Execute(ItemExecutionContext context)
            => context.target.ApplyModifier(stat, modifier);
    }
}
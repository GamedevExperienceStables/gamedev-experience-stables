﻿using Game.Actors;
using UnityEngine;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Rune")]
    public class RuneDefinition : ItemInventoryDefinition, IItemExecutableDefinition
    {
        [SerializeField]
        private Sprite icon;
        
        [SerializeField]
        private Sprite iconActive;
        
        [SerializeField]
        private AbilityDefinition grantAbility;
        
        [SerializeField]
        private RuneType type = RuneType.Active;

        public Sprite Icon => icon;
        public Sprite IconActive => iconActive;
        public AbilityDefinition GrantAbility => grantAbility;
        public RuneType Type => type;

        public bool CanExecute(ItemExecutionContext context)
            => IsAbilityExistsAndDisabled(grantAbility, context.target);

        public void Execute(ItemExecutionContext context) 
            => context.target.GiveAbility(grantAbility);

        private static bool IsAbilityExistsAndDisabled(AbilityDefinition definition, IActorController target)
        {
            if (!target.TryGetAbility(definition, out ActorAbility ability))
                return false;

            return !ability.IsEnabled;
        }
    }
}
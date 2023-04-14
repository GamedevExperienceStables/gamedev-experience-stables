﻿using UnityEngine;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Recipe")]
    public class RecipeDefinition : ConsumableDefinition
    {
        [SerializeField]
        private RuneDefinition grantsRune;

        public override bool CanExecute(ItemExecutionContext context)
            => context.inventory.CanAddToBag(grantsRune, context.target);

        public override void Execute(ItemExecutionContext context)
        {
            if (!context.inventory.CanAddToBag(grantsRune, context.target))
            {
                Debug.LogWarning($"Can't grant rune '{grantsRune.name}'");
                return;
            }

            context.inventory.AddToBag(grantsRune, context.target);
        }
    }
}
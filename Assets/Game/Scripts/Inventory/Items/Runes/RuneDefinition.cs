﻿using Game.Actors;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Rune")]
    public class RuneDefinition : ItemInventoryDefinition, IItemExecutableDefinition
    {
        [SerializeField]
        private LocalizedString name;
        
        [SerializeField]
        private LocalizedString description;
        
        [Header("Icons")]
        [SerializeField]
        private Sprite icon;
        
        [SerializeField]
        private Sprite iconActive;
        
        [SerializeField]
        private Sprite iconEmpty;

        [Header("Additional")]
        [SerializeField]
        private RuneType type = RuneType.Active;

        [SerializeField, ShowIf(nameof(type), RuneType.Active)]
        [ColorUsage(true, true)]
        private Color color = Color.white;

        [SerializeField]
        private RuneLevelDefinition level;

        [Header("Ability")]
        [SerializeField]
        private AbilityDefinition grantAbility;

        public Sprite Icon => icon;
        public Sprite IconActive => iconActive;
        public Sprite IconEmpty => iconEmpty;
        public LocalizedString Name => name;
        public LocalizedString Description => description;
        public RuneLevelDefinition Level => level;
        
        public AbilityDefinition GrantAbility => grantAbility;
        public RuneType Type => type;
        public Color Color => color;

        public bool CanExecute(ItemExecutionContext context)
            => true;

        public void Execute(ItemExecutionContext context)
        {
            if (IsAbilityExistsAndDisabled(grantAbility, context.target))
                context.target.GiveAbility(grantAbility);
        }

        private static bool IsAbilityExistsAndDisabled(AbilityDefinition definition, IActorController target)
        {
            if (!target.TryGetAbility(definition, out ActorAbility ability))
                return false;

            return !ability.IsEnabled;
        }
    }
}
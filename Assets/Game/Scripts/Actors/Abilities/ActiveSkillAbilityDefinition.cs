using Game.Inventory;
using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Active Skill")]
    public class ActiveSkillAbilityDefinition : AbilityDefinition<ActiveSkillAbility>
    {
        [SerializeField]
        private AbilityDefinition defaultAbility;

        public AbilityDefinition DefaultAbility => defaultAbility;
    }

    public class ActiveSkillAbility : ActorAbility<ActiveSkillAbilityDefinition>
    {
        private readonly IInventorySlots _slots;
        private ActorAbility _activeAbility;

        private AimAbility _aim;
        private ActorAbility _defaultAbility;

        public ActiveSkillAbility(IInventorySlots slots)
            => _slots = slots;


        protected override void OnInitAbility()
        {
            _slots.ActiveSlotChanged += OnActiveSlotChanged;

            _aim = Owner.GetAbility<AimAbility>();

            SetDefaultAbility();
        }

        protected override void OnDestroyAbility()
            => _slots.ActiveSlotChanged -= OnActiveSlotChanged;

        private void SetDefaultAbility()
        {
            if (TryFindAbility(Definition.DefaultAbility, out _defaultAbility))
                _activeAbility = _defaultAbility;
        }

        private bool TryFindAbility(AbilityDefinition definition, out ActorAbility foundAbility)
        {
            if (Owner.TryGetAbility(definition, out foundAbility))
                return true;

            Debug.LogWarning($"Not found ability: {definition}");
            return false;
        }

        public override bool CanActivateAbility()
            => _aim.IsActive;

        protected override void OnActivateAbility()
        {
            _activeAbility.TryActivateAbility();
            EndAbility();
        }

        private void OnActiveSlotChanged(RuneActiveSlotChangedEvent changed)
        {
            if (_slots.HasActive)
            {
                AbilityDefinition runeDefinition = _slots.ActiveSlot.Rune.GrantAbility;
                if (TryFindAbility(runeDefinition, out ActorAbility foundAbility))
                {
                    _activeAbility = foundAbility;
                    return;
                }
            }

            _activeAbility = _defaultAbility;
        }
    }
}
using Game.Inventory;
using Game.Weapons;
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
        private IActorInputController _input;

        public ActiveSkillAbility(IInventorySlots slots)
            => _slots = slots;

        public bool IsGroundTargeting { get; private set; }


        protected override void OnInitAbility()
        {
            _slots.ActiveSlotChanged += OnActiveSlotChanged;

            _aim = Owner.GetAbility<AimAbility>();
            _input = Owner.GetComponent<IActorInputController>();
            
            SetDefaultAbility();
        }

        protected override void OnDestroyAbility()
            => _slots.ActiveSlotChanged -= OnActiveSlotChanged;

        private void SetDefaultAbility()
        {
            if (TryFindAbility(Definition.DefaultAbility, out _defaultAbility))
                SetActiveAbility(_defaultAbility);
        }

        private bool TryFindAbility(AbilityDefinition definition, out ActorAbility foundAbility)
        {
            if (Owner.TryGetAbility(definition, out foundAbility))
                return true;

            Debug.LogWarning($"Not found ability: {definition}");
            return false;
        }

        public override bool CanActivateAbility()
        {
            if (!_aim.IsActive)
                return false;

            return !_input.HasAnyBlock(InputBlock.Action);
        }

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
                    SetActiveAbility(foundAbility);
                    return;
                }
            }

            SetActiveAbility(_defaultAbility);
        }

        private void SetActiveAbility(ActorAbility ability)
        {
            _activeAbility = ability;
            
            UpdateTargetingType();
            _aim.UpdateState();
        }


        private void UpdateTargetingType()
        {
            if (_activeAbility.Definition is not ProjectileAbilityDefinition projectileDefinition)
            {
                IsGroundTargeting = false;
                return;
            }

            IsGroundTargeting = projectileDefinition.Projectile.Trajectory is ProjectileTrajectoryBallistic;
        }
    }
}
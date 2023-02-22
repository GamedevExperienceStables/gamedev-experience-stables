using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Weapon")]
    public class WeaponAbilityDefinition : AbilityDefinition<WeaponAbility>
    {
        [SerializeField]
        private bool isAimAbilityRequired;
        [SerializeField]
        private LayerMask mask;
        public bool IsAimAbilityRequired => isAimAbilityRequired;
        public LayerMask Mask => mask;
    }

    public class WeaponAbility : ActorAbility<WeaponAbilityDefinition>
    {
        private AimAbility _aim;
        private ProjectileWeapon _currentWeapon;

        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();

            var view = Owner.GetComponent<WeaponAbilityView>();
            _currentWeapon = view.CurrentWeapon;
            _currentWeapon.SetOwner(Owner);
        }

        public override bool CanActivateAbility()
            => !Definition.IsAimAbilityRequired || _aim.IsActive;

        protected override void OnActivateAbility()
            => _currentWeapon.SpawnProjectile(_aim.GetRealPosition());
    }
}
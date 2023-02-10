using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Weapon")]
    public class WeaponAbilityDefinition : AbilityDefinition<WeaponAbility>
    {
        [SerializeField]
        private bool isEnemy;
        [SerializeField]
        private LayerMask mask;
        public bool IsEnemy => isEnemy;
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
            => Definition.IsEnemy || _aim.IsActive;

        protected override void OnActivateAbility()
            => _currentWeapon.SpawnProjectile();
    }
}
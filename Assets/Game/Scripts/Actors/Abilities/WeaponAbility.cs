using Game.Weapons;
using UnityEngine;

namespace Game.Actors
{
    public class WeaponAbility : ActorAbilityView
    {
        [SerializeField]
        private ProjectileWeapon currentWeapon;

        private AimAbility _aim;

        protected override void OnInitAbility()
        {
            _aim = Owner.FindAbility<AimAbility>();

            currentWeapon.SetOwner(Owner);
        }

        public override bool CanActivateAbility()
            => _aim.IsActive;

        protected override void OnActivateAbility()
            => currentWeapon.SpawnProjectile();
    }
}
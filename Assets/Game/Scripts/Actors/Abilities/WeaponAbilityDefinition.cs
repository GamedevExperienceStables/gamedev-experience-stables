    using System;
    using Cysharp.Threading.Tasks;
    using Game.Weapons;
using UnityEngine;
using UnityEngine.Serialization;

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
        private Animator _animator;
        private bool _isAnimationEnded;

        protected override void OnInitAbility()
        {
            _aim = Owner.GetAbility<AimAbility>();
            var view = Owner.GetComponent<WeaponAbilityView>();
            _animator = Owner.GetComponent<Animator>();
            _currentWeapon = view.CurrentWeapon;
            _currentWeapon.SetOwner(Owner);
            _isAnimationEnded = true;
        }

        public override bool CanActivateAbility()
            => ((!Definition.IsAimAbilityRequired || _aim.IsActive) && _isAnimationEnded);

        protected override async void OnActivateAbility()
        {
            if (_animator != null)
            {
                _animator.SetBool("IsAttacked", true);
                _isAnimationEnded = false;
                await WaitAnimationEnd();
                _animator.SetBool("IsAttacked", false);
            }
            _currentWeapon.SpawnProjectile();
        }

        private async UniTask WaitAnimationEnd()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.5f), ignoreTimeScale: false);
            _isAnimationEnded = true;
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using Game.Actors.Health;
using Game.Animations.Hero;
using Game.Stats;
using Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "MeleeAbility")]
    public class MeleeAbilityDefinition : AbilityDefinition<MeleeAbility>
    {
        [SerializeField, Min(0)]
        private float baseDamage;

        [SerializeField, Min(0)]
        private float staminaCost;

        [Space]
        [FormerlySerializedAs("MeleeAtkArea")]
        [SerializeField]
        private float meleeRangeRadius;

        [SerializeField]
        private float pushForce;

        [SerializeField]
        private int maxTargets;

        [SerializeField]
        private Vector3 sphereColliderShift;

        [Space]
        [SerializeField]
        private LayerMask mask;

        public float MeleeRangeRadius => meleeRangeRadius;
        public float MeleeDamage => baseDamage;
        public float StaminaCost => staminaCost;
        public float PushForce => pushForce;
        public LayerMask Mask => mask;
        public Vector3 SphereColliderShift => sphereColliderShift;
        public int MaxTargets => maxTargets;
    }

    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private bool _hasAim;
        private AimAbility _aim;

        private Collider[] _hitColliders;
        private ActorAnimator _animator;
        private bool _isAnimationEnded;
        private IActorInputController _inputController;

        private bool _hasDamageModifier;
        private bool _hasStaminaModifier;


        public override bool CanActivateAbility()
        {
            if (!_isAnimationEnded)
                return false;

            if (_hasAim && !_aim.IsActive)
                return false;

            return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(GetCost());
        }

        protected override void OnInitAbility()
        {
            _inputController = Owner.GetComponent<IActorInputController>();

            _hasAim = Owner.TryGetAbility(out _aim);

            _hitColliders = new Collider[Definition.MaxTargets];
            _animator = Owner.GetComponent<ActorAnimator>();
            _isAnimationEnded = true;

            _hasDamageModifier = Owner.HasStat(CharacterStats.MeleeDamageMultiplier);
            _hasStaminaModifier = Owner.HasStat(CharacterStats.MeleeStaminaMultiplier);
        }

        protected override async void OnActivateAbility()
        {
            _inputController.BlockInput(true);
            
            bool isEnded = await AbilityAnimation();
            if (isEnded)
            {
                return;
            }
            _animator.SetAnimation(AnimationNames.MeleeAttack, false);
            Owner.ApplyModifier(CharacterStats.Stamina, -GetCost());
            Vector3 sphereShift = Owner.Transform.position + Definition.SphereColliderShift;
#if UNITY_EDITOR
            DebugExtensions.DebugWireSphere(sphereShift, radius: Definition.MeleeRangeRadius);
#endif
            int numColliders = Physics.OverlapSphereNonAlloc(sphereShift,
                Definition.MeleeRangeRadius, _hitColliders, Definition.Mask);
            for (int i = 0; i < numColliders; i++)
            {
                Transform hit = _hitColliders[i].transform;
                Debug.Log(hit.gameObject + "MELEE ATTACKED");
                if (hit.gameObject.TryGetComponent(out IActorController destinationOwner))
                    destinationOwner.GetComponent<DamageableController>().Damage(GetDamage());
                Vector3 dir = hit.position - Owner.Transform.position;
                dir = dir.normalized * Definition.PushForce;
                hit.GetComponent<MovementController>().AddVelocity(dir);
            }
        }

        private async UniTask<bool> WaitAnimationEnd()
        {
            bool isCancelled = await UniTask.Delay(TimeSpan.FromSeconds(0.75f), 
                ignoreTimeScale: false, 
                cancellationToken: Owner.CancellationToken()).SuppressCancellationThrow();
            _isAnimationEnded = true;
            _inputController.BlockInput(false);
            return isCancelled;
        }

        private async UniTask<bool> AbilityAnimation()
        {
            _animator.SetAnimation(AnimationNames.MeleeAttack, true);
            _isAnimationEnded = false;
            bool isEnded = await WaitAnimationEnd();
            return isEnded;
        }

        private float GetCost()
        {
            float baseCost = Definition.StaminaCost;
            if (!_hasStaminaModifier)
                return baseCost;

            float modifier = baseCost * Owner.GetCurrentValue(CharacterStats.MeleeStaminaMultiplier);
            float cost = baseCost - +modifier;
            return cost;
        }

        private float GetDamage()
        {
            float baseDamage = Definition.MeleeDamage;
            if (!_hasDamageModifier)
                return baseDamage;

            float modifier = baseDamage * Owner.GetCurrentValue(CharacterStats.MeleeStaminaMultiplier);
            float damage = baseDamage + modifier;
            return damage;
        }
    }
}
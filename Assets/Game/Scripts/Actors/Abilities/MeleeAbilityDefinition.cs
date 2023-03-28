﻿using System;
using Game.Actors.Health;
using Game.Animations.Hero;
using Game.Stats;
using Game.TimeManagement;
using Game.Utils;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using Object = UnityEngine.Object;

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

        [Space]
        [SerializeField]
        private float castTime = 0.75f;

        [SerializeField]
        private GameObject completeEffectPrefab;

        public float MeleeRangeRadius => meleeRangeRadius;
        public float MeleeDamage => baseDamage;
        public float StaminaCost => staminaCost;
        public float PushForce => pushForce;
        public LayerMask Mask => mask;
        public Vector3 SphereColliderShift => sphereColliderShift;
        public int MaxTargets => maxTargets;

        public float CastTime => castTime;

        public GameObject CompleteEffectPrefab => completeEffectPrefab;
    }

    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private readonly TimerPool _timers;

        private Collider[] _hitColliders;
        private ActorAnimator _animator;
        private IActorInputController _inputController;

        private bool _hasDamageModifier;
        private bool _hasStaminaModifier;

        private TimerUpdatable _castTimer;
        
        private bool _hasCompleteEffect;

        [Inject]
        public MeleeAbility(TimerPool timers)
            => _timers = timers;

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            return Owner.GetCurrentValue(CharacterStats.Stamina) >= Mathf.Abs(GetCost());
        }

        protected override void OnInitAbility()
        {
            _inputController = Owner.GetComponent<IActorInputController>();

            _hitColliders = new Collider[Definition.MaxTargets];
            _animator = Owner.GetComponent<ActorAnimator>();

            _hasDamageModifier = Owner.HasStat(CharacterStats.MeleeDamageMultiplier);
            _hasStaminaModifier = Owner.HasStat(CharacterStats.MeleeStaminaMultiplier);

            _castTimer = _timers.GetTimer(TimeSpan.FromSeconds(Definition.CastTime), OnComplete);

            _hasCompleteEffect = Definition.CompleteEffectPrefab;
        }

        protected override void OnActivateAbility()
        {
            Owner.ApplyModifier(CharacterStats.Stamina, -GetCost());

            _inputController.BlockInput(true);
            SetAnimation(true);

            _castTimer.Start();
        }

        private void OnComplete()
        {
            MakeDamage();
            SpawnEffect();

            EndAbility();
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _inputController.BlockInput(false);
            SetAnimation(false);

            _castTimer?.Stop();
        }

        private void MakeDamage()
        {
            Vector3 sphereShift = Owner.Transform.position + Definition.SphereColliderShift;
#if UNITY_EDITOR
            DebugExtensions.DebugWireSphere(sphereShift, radius: Definition.MeleeRangeRadius);
#endif
            int numColliders = Physics.OverlapSphereNonAlloc(sphereShift,
                Definition.MeleeRangeRadius, _hitColliders, Definition.Mask);
            for (int i = 0; i < numColliders; i++)
            {
                Transform hit = _hitColliders[i].transform;
                if (hit.gameObject.TryGetComponent(out IActorController destinationOwner))
                    destinationOwner.GetComponent<DamageableController>().Damage(GetDamage());
                Vector3 dir = hit.position - Owner.Transform.position;
                dir = dir.normalized * Definition.PushForce;
                hit.GetComponent<MovementController>().AddVelocity(dir);
            }
        }

        private void SetAnimation(bool isActive)
            => _animator.SetAnimation(AnimationNames.MeleeAttack, isActive);

        private float GetCost()
        {
            float baseCost = Definition.StaminaCost;
            return _hasStaminaModifier
                ? baseCost.AddPercent(Owner.GetCurrentValue(CharacterStats.MeleeStaminaMultiplier))
                : baseCost;
        }

        private float GetDamage()
        {
            float baseDamage = Definition.MeleeDamage;
            return _hasDamageModifier
                ? baseDamage.AddPercent(Owner.GetCurrentValue(CharacterStats.MeleeDamageMultiplier))
                : baseDamage;
        }

        private void SpawnEffect()
        {
            if (_hasCompleteEffect)
                Object.Instantiate(Definition.CompleteEffectPrefab, Owner.Transform.position, Quaternion.identity);
        }
    }
}
using System;
using Game.Actors.Health;
using Game.Animations.Hero;
using Game.Level;
using Game.Stats;
using Game.TimeManagement;
using Game.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

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
        [SerializeField, Min(0)]
        private float castTime = 0.75f;

        [SerializeField, Min(0)]
        private float animationSpeedMultiplier = 1f;

        [Header("FX")]
        [SerializeField, Expandable]
        private MeleeAbilityFx visualEffects;

        public float MeleeRangeRadius => meleeRangeRadius;
        public float MeleeDamage => baseDamage;
        public float StaminaCost => staminaCost;
        public float PushForce => pushForce;
        public LayerMask Mask => mask;
        public Vector3 SphereColliderShift => sphereColliderShift;
        public int MaxTargets => maxTargets;

        public float CastTime => castTime;

        public float AnimationSpeedMultiplier => animationSpeedMultiplier;

        public MeleeAbilityFx VisualEffects => visualEffects;
    }

    public class MeleeAbility : ActorAbility<MeleeAbilityDefinition>
    {
        private readonly TimerPool _timers;
        private readonly SpawnPool _spawnPool;

        private Collider[] _hitColliders;
        private ActorAnimator _animator;
        private IActorInputController _inputController;

        private bool _hasDamageModifier;
        private bool _hasStaminaModifier;

        private TimerUpdatable _castTimer;

        private bool _hasStartEffect;
        private bool _hasCompleteEffect;
        private bool _hasHitEffect;

        [Inject]
        public MeleeAbility(TimerPool timers, SpawnPool spawnPool)
        {
            _timers = timers;
            _spawnPool = spawnPool;
        }

        public override bool CanActivateAbility()
        {
            if (IsActive)
                return false;

            if (_inputController.HasAnyBlock(InputBlock.Action))
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

            _hasStartEffect = Definition.VisualEffects && Definition.VisualEffects.OnStart;
            _hasCompleteEffect = Definition.VisualEffects && Definition.VisualEffects.OnEnd;
            _hasHitEffect = Definition.VisualEffects && Definition.VisualEffects.OnHit;

            if (_hasStartEffect)
                _spawnPool.Prewarm(Definition.VisualEffects.OnStart);
            
            if (_hasCompleteEffect)
                _spawnPool.Prewarm(Definition.VisualEffects.OnEnd);
            
            if (_hasHitEffect)
                _spawnPool.Prewarm(Definition.VisualEffects.OnHit);
        }

        protected override void OnDestroyAbility()
            => _timers.ReleaseTimer(_castTimer);

        protected override void OnActivateAbility()
        {
            SpawnStartEffect();

            Owner.ApplyModifier(CharacterStats.Stamina, -GetCost());

            _inputController.SetBlock(true);
            SetAnimation(true);

            _castTimer.Start();
        }

        private void OnComplete()
        {
            MakeDamage();
            SpawnCompleteEffect();

            EndAbility();
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _inputController.SetBlock(false);
            SetAnimation(false);

            _castTimer?.Stop();
        }

        private void MakeDamage()
        {
            Vector3 sphereShift = Owner.Transform.position +
                                  Owner.Transform.TransformDirection(Definition.SphereColliderShift);
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

                Vector3 hitPosition = hit.position;

                if (Definition.PushForce > 0)
                {
                    Vector3 dir = hitPosition - Owner.Transform.position;
                    dir = dir.normalized * Definition.PushForce;
                    hit.GetComponent<MovementController>().AddVelocity(dir);
                }

                SpawnHitEffect(hit);
            }
        }

        private void SetAnimation(bool isActive)
        {
            float animationSpeed = isActive ? Definition.AnimationSpeedMultiplier : 1f;
            _animator.SetAnimation(AnimationNames.AttackSpeedMultiplier, animationSpeed);
            _animator.SetAnimation(AnimationNames.MeleeAttack, isActive);
        }

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

        private void SpawnCompleteEffect()
        {
            if (_hasCompleteEffect)
                SpawnEffect(Definition.VisualEffects.OnEnd);
        }

        private void SpawnHitEffect(Transform hit)
        {
            if (!_hasHitEffect)
                return;

            Vector3 spawnPoint = hit.position + hit.TransformDirection(Definition.VisualEffects.OnHitOffset);
            Quaternion spawnRotation = Quaternion.LookRotation(hit.forward);
            SpawnEffect(Definition.VisualEffects.OnHit, spawnPoint, spawnRotation);
        }

        private void SpawnStartEffect()
        {
            if (_hasStartEffect)
                SpawnEffect(Definition.VisualEffects.OnStart);
        }

        private void SpawnEffect(GameObject effectPrefab)
        {
            Vector3 spawnPoint = Owner.Transform.position;
            Quaternion spawnRotation = Quaternion.LookRotation(Owner.Transform.forward);
            SpawnEffect(effectPrefab, spawnPoint, spawnRotation);
        }

        private void SpawnEffect(GameObject effectPrefab, Vector3 spawnPoint, Quaternion spawnRotation) 
            => _spawnPool.Spawn(effectPrefab, spawnPoint, spawnRotation);
    }
}
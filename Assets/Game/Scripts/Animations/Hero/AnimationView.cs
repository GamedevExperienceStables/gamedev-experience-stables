﻿using Game.Actors;
using Game.Actors.Health;
using UnityEngine;

namespace Game.Animations.Hero
{
    public class AnimationView : MonoBehaviour
    {
        [SerializeField]
        private MovementController movementController;
        private ActorAnimator _animator;
        private DeathController _deathController;
        private DamageableController _damageableController;

        private void Awake()
        {
            _animator = GetComponent<ActorAnimator>();
            _deathController = GetComponent<DeathController>();
            _damageableController = GetComponent<DamageableController>();
            
            _deathController.Died += OnDiedAnimation;
            _deathController.Revived += OnRevivedAnimation;
            _damageableController.DamageFeedback += OnDamageAnimation;
        }

        private void OnDestroy()
        {
            _deathController.Died -= OnDiedAnimation;
            _deathController.Revived -= OnRevivedAnimation;
            _damageableController.DamageFeedback -= OnDamageAnimation;
        }

        private void Update()
        {
            MovementAnimation();
        }

        private void MovementAnimation()
        {
            Vector3 direction = movementController.GetMovementDirection();
            _animator.SetAnimation(AnimationNames.XCoordinate, direction.x);
            _animator.SetAnimation(AnimationNames.YCoordinate, direction.z);
        }

        private void OnDiedAnimation()
        {
            _animator.ResetAnimation(AnimationNames.Damage);
            _animator.SetAnimation(AnimationNames.Death, true);
        }

        private void OnRevivedAnimation() 
            => _animator.SetAnimation(AnimationNames.Death, false);

        private void OnDamageAnimation()
        {
            _animator.SetAnimation(AnimationNames.Damage);
        }
        
        
        
    }
}
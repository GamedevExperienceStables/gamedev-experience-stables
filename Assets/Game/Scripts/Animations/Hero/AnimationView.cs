using System;
using Cysharp.Threading.Tasks;
using Game.Actors;
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
        private bool _isDied;

        private void Awake()
        {
            _animator = GetComponent<ActorAnimator>();
            _deathController = GetComponent<DeathController>();
            _damageableController = GetComponent<DamageableController>();
            
            _deathController.Died += OnDiedAnimation;
            _damageableController.DamageFeedback += OnDamageAnimation;
        }

        private void Update()
        {
            MovementAnimation();
        }

        private void MovementAnimation()
        {
            Vector3 direction = movementController.GetMovementDirection();
            Debug.Log(direction);
            _animator.SetAnimation(AnimationNames.XCoordinate, direction.x);
            _animator.SetAnimation(AnimationNames.YCoordinate, direction.z);
        }

        private void OnDiedAnimation()
        {
            _isDied = true;
            _animator.ResetAnimation(AnimationNames.Damage);
            _animator.SetAnimation(AnimationNames.Death);
        }
        
        private void OnDamageAnimation()
        {
            if (_isDied) return;
            _animator.SetAnimation(AnimationNames.Damage);
        }
        
        
        
    }
}
using System;
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

        private void Awake()
        {
            _animator = GetComponent<ActorAnimator>();
            _deathController = GetComponent<DeathController>();
            _deathController.Died += OnDiedAnimation;
        }

        private void Update()
        {
            MovementAnimation();
        }

        private void MovementAnimation()
        {
            var direction = movementController.GetMovementDirection();
            _animator.SetAnimation(AnimationNames.XCoordinate, direction.x);
            _animator.SetAnimation(AnimationNames.YCoordinate, direction.z);
        }

        private void OnDiedAnimation()
        {
            _animator.PlayAnimation(AnimationNames.Death);
        }
    }
}
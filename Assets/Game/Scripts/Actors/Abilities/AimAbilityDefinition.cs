﻿using Game.Animations.Hero;
using Game.Cameras;
using Game.Hero;
using Game.Stats;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = MENU_PATH + "Aim")]
    public class AimAbilityDefinition : AbilityDefinition<AimAbility>
    {
        [SerializeField]
        private StatModifier speedModifier;

        public StatModifier SpeedModifier => speedModifier;
    }

    public class AimAbility : ActorAbility<AimAbilityDefinition>
    {
        private readonly FollowSceneCamera _followCamera;
        private ActorAnimator _animator;
        private HeroInputController _heroInputController;
        private bool _isHeroNull;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera)
            => _followCamera = followCamera;

        public override bool CanActivateAbility()
            => true;

        protected override void OnInitAbility()
        {
            _animator = Owner.GetComponent<ActorAnimator>();
            _heroInputController = Owner.GetComponent<HeroInputController>();
            _isHeroNull = _heroInputController != null;
        }
        
        protected override void OnActivateAbility()
        {
            _animator.SetAnimation(AnimationNames.Aiming, true);
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _animator.SetAnimation(AnimationNames.Aiming, false);
            _followCamera.ZoomReset();
            Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }
        
        public Vector3 GetRealPosition()
        {
            return _isHeroNull ? _heroInputController.GetRealMousePosition() : default;
        }
    }
}
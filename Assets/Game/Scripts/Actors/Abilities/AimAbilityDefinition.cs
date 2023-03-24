using Game.Animations.Hero;
using Game.Cameras;
using Game.CursorManagement;
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
        private readonly CursorService _cursor;
        
        private ActorAnimator _animator;
        private HeroInputController _heroInputController;
        private bool _isHero;
        private ActiveSkillAbility _activeSkillAbility;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera, CursorService cursor)
        {
            _followCamera = followCamera;
            _cursor = cursor;
        }

        public override bool CanActivateAbility()
            => true;

        protected override void OnInitAbility()
        {
            _animator = Owner.GetComponent<ActorAnimator>();
            _heroInputController = Owner.GetComponent<HeroInputController>();
            _activeSkillAbility = Owner.GetAbility<ActiveSkillAbility>();
            _isHero = _heroInputController;
        }
        
        protected override void OnActivateAbility()
        {
            HandleTargeting();

            _animator.SetAnimation(AnimationNames.Aiming, true);
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        private void HandleTargeting()
        {
            var def = _activeSkillAbility.ActiveAbility.Definition as ProjectileAbilityDefinition;
            if (def && def.IsCursorInvisible)
            {
                _cursor.SetVisible(false);
                _heroInputController.SetTargetingVisible(true);
            }
            else
            {
                _cursor.SetVisible(true);
                _cursor.SetAlternative();
                _heroInputController.SetTargetingVisible(false);
            }
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _cursor.Reset();
            
            _animator.SetAnimation(AnimationNames.Aiming, false);
            _followCamera.ZoomReset();
            Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }
        
        public Vector3 GetRealPosition()
        {
            return _isHero ? _heroInputController.GetRealMousePosition() : default;
        }
    }
}
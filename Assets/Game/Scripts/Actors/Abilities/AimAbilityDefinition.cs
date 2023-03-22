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
        private bool _isHeroNull;
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
            _isHeroNull = _heroInputController != null;
        }
        
        protected override void OnActivateAbility()
        {
            ProjectileAbilityDefinition def = _activeSkillAbility.ActiveAbility.Definition as ProjectileAbilityDefinition;
            if (def != null) _cursor.SetVisible(!def.IsCursorInvisible);

            _cursor.SetAlternative();
         
            _animator.SetAnimation(AnimationNames.Aiming, true);
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
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
            return _isHeroNull ? _heroInputController.GetRealMousePosition() : default;
        }
    }
}
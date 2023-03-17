using Game.Animations.Hero;
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
        
        [SerializeField]
        private Texture2D cursorTexture;

        public StatModifier SpeedModifier => speedModifier;
        public Texture2D CursorTexture => cursorTexture;
    }

    public class AimAbility : ActorAbility<AimAbilityDefinition>
    {
        private readonly FollowSceneCamera _followCamera;
        private ActorAnimator _animator;
        private HeroInputController _heroInputController;
        private bool _isHeroNull;
        private ActiveSkillAbility _activeSkillAbility;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera)
            => _followCamera = followCamera;

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
            if (def != null) Cursor.visible = !def.IsCursorInvisible;
            Cursor.SetCursor(Definition.CursorTexture, Vector2.zero, CursorMode.Auto);
            _animator.SetAnimation(AnimationNames.Aiming, true);
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            Cursor.visible = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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
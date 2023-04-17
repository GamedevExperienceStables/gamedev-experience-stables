using Game.Animations.Hero;
using Game.Cameras;
using Game.CursorManagement;
using Game.Stats;
using Game.Utils;
using Game.Weapons;
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
        private Reticle groundReticle;

        public StatModifier SpeedModifier => speedModifier;

        public Reticle GroundReticle => groundReticle;
    }

    public class AimAbility : ActorAbility<AimAbilityDefinition>
    {
        private readonly FollowSceneCamera _followCamera;
        private readonly CursorService _cursor;
        private readonly GameplayPrefabFactory _prefabFactory;

        private ActorAnimator _animator;
        private ActiveSkillAbility _activeSkillAbility;

        private Reticle _reticle;
        private IActorInputController _input;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera, CursorService cursor, GameplayPrefabFactory prefabFactory)
        {
            _followCamera = followCamera;
            _cursor = cursor;
            _prefabFactory = prefabFactory;
        }

        public void UpdateState()
        {
            if (IsActive)
                UpdateTargeting();
        }

        public override bool CanActivateAbility() 
            => !_input.HasAnyBlock(InputBlock.Action);

        protected override void OnInitAbility()
        {
            _animator = Owner.GetComponent<ActorAnimator>();
            _activeSkillAbility = Owner.GetAbility<ActiveSkillAbility>();
            _input = Owner.GetComponent<IActorInputController>();

            CreateGroundReticle();
        }

        protected override void OnActivateAbility()
        {
            UpdateTargeting();

            _animator.SetAnimation(AnimationNames.Aiming, true);
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        protected override void OnEndAbility(bool wasCancelled)
        {
            _reticle.Hide();
            _cursor.Reset();

            _animator.SetAnimation(AnimationNames.Aiming, false);
            _followCamera.ZoomReset();
            Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        private void UpdateTargeting()
        {
            if (_activeSkillAbility.IsGroundTargeting)
            {
                _reticle.Show();
                _cursor.SetVisible(false);
            }
            else
            {
                _reticle.Hide();
                _cursor.SetVisible(true);
                _cursor.SetAlternative();
            }
        }

        private void CreateGroundReticle()
        {
            _reticle = _prefabFactory.Create(Definition.GroundReticle);
            _reticle.Init(OnUpdatePositionReticle);
            _reticle.Hide();
        }

        private void OnUpdatePositionReticle()
        {
            Vector3 targetPosition = _input.GetTargetPosition(grounded: true);
            _reticle.SetPosition(targetPosition);
        }
    }
}
using Game.Cameras;
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
        private IMovableStats _movableStats;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera)
            => _followCamera = followCamera;

        public override bool CanActivateAbility()
            => true;

        protected override void OnInitAbility()
            => _movableStats = Owner.GetStats<IMovableStats>();

        protected override void OnActivateAbility()
        {
            _followCamera.ZoomOut();
            _movableStats.Movement.AddModifier(Definition.SpeedModifier);
        }

        protected override void OnEndAbility()
        {
            _followCamera.ZoomReset();
            _movableStats.Movement.RemoveModifier(Definition.SpeedModifier);
        }
    }
}
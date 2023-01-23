using System;
using Game.Cameras;
using Game.Stats;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    public class AimAbility : ActorAbilityView
    {
        private FollowSceneCamera _followCamera;
        private IMovableStats _movableStats;
        private StatModifier _speedModifier;

        [Inject]
        public void Construct(Settings settings, FollowSceneCamera followCamera)
        {
            _speedModifier = settings.SpeedModifier;
            _followCamera = followCamera;
        }

        public override bool CanActivateAbility()
            => true;

        protected override void OnInitAbility()
            => _movableStats = Owner.GetStats<IMovableStats>();

        protected override void OnActivateAbility()
        {
            _followCamera.ZoomOut();
            _movableStats.Movement.AddModifier(_speedModifier);
        }

        protected override void OnEndAbility()
        {
            _followCamera.ZoomReset();
            _movableStats.Movement.RemoveModifier(_speedModifier);
        }

        [Serializable]
        public class Settings
        {
            [SerializeField]
            private StatModifier speedModifier;

            public StatModifier SpeedModifier => speedModifier;
        }
    }
}
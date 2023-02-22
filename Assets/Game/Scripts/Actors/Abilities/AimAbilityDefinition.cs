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
        private HeroInputController _heroInputController;

        [Inject]
        public AimAbility(FollowSceneCamera followCamera)
        {
            _followCamera = followCamera;
        }

        public override bool CanActivateAbility()
            => true;

        protected override void OnInitAbility()
        {
            _heroInputController = Owner.GetComponent<HeroInputController>();
        }
        
        protected override void OnActivateAbility()
        {
            _followCamera.ZoomOut();
            Owner.AddModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }

        public Vector3 GetRealPosition() => _heroInputController.GetRealMousePosition();
        
        protected override void OnEndAbility(bool wasCancelled)
        {
            _followCamera.ZoomReset();
            Owner.RemoveModifier(CharacterStats.MovementSpeed, Definition.SpeedModifier);
        }
    }
}
using Game.Actors;
using Game.Stats;
using UnityEngine;

namespace Game.Hero
{
    public class HeroController : ActorController
    {
        private MovementController _movement;
        private HeroView _view;

        private IHeroStats _stats;

        protected override void OnAwake()
        {
            _view = GetComponent<HeroView>();
            _movement = GetComponent<MovementController>();
        }

        public override IStatsSet Stats => _stats;
        public Transform CameraTarget => _view.CameraTarget;

        public void SetStats(IHeroStats stats)
            => _stats = stats;

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
            => _movement.SetPositionAndRotation(position, rotation);

        public void SetActive(bool value)
            => gameObject.SetActive(value);

        public void Reset() 
            => ResetAbilities();
    }
}
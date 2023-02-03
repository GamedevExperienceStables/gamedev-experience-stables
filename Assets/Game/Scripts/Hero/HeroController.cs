using Game.Actors;
using Game.Level;
using Game.Stats;
using UnityEngine;

namespace Game.Hero
{
    public class HeroController : ActorController, ISpawnZoneTrigger
    {
        private MovementController _movement;
        private HeroView _view;
        
        private HeroStats _stats;

        protected override IStats Stats => _stats;

        protected override void OnActorAwake()
        {
            _view = GetComponent<HeroView>();
            _movement = GetComponent<MovementController>();
        }

        public void Bind(HeroStats stats) 
            => _stats = stats;

        public Transform CameraTarget => _view.CameraTarget;

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
            => _movement.SetPositionAndRotation(position, rotation);

        public void SetActive(bool value)
            => gameObject.SetActive(value);

        public void Reset() 
            => ResetAbilities();
    }
}
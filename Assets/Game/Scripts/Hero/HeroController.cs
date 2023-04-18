using Game.Actors;
using Game.Level;
using Game.Pet;
using Game.Stats;
using UnityEngine;

namespace Game.Hero
{
    public class HeroController : ActorController, ISpawnZoneTrigger
    {
        private PetController _pet;
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
        {
            _movement.SetPositionAndRotation(position, rotation);

            if (_pet)
                _pet.ResetPosition();
        }

        public void BindPet(PetController pet)
        {
            _pet = pet;
            _pet.SetFollowingPositions(_view.PetPoints);
        }

        public void SetActive(bool value)
            => gameObject.SetActive(value);

        public void Reset() 
            => ResetAbilities();

        public bool CanActivate(AbilityDefinition runeGrantAbility)
            => TryGetAbility(runeGrantAbility, out ActorAbility foundAbility) && foundAbility.CanActivateAbility();
    }
}
using Game.Actors;
using Game.Cameras;
using Game.Input;
using Game.Level;
using UnityEngine;
using VContainer;

namespace Game.Hero
{
    public class HeroFactory
    {
        private readonly IInputControlGameplay _input;
        private readonly InteractionService _interactionService;
        private readonly HeroDefinition _heroData;

        [Inject]
        public HeroFactory(IInputControlGameplay input, InteractionService interactionService, HeroDefinition heroData)
        {
            _input = input;
            _interactionService = interactionService;
            _heroData = heroData;
        }

        public HeroController Create(Transform spawnPoint, SceneCamera sceneCamera, FollowSceneCamera followCamera)
        {
            HeroController hero = Object.Instantiate(_heroData.Prefab);
            hero.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            hero.Init(_input, sceneCamera, followCamera);

            var movement = hero.GetComponent<MovementController>();
            movement.SetMovementSpeed(_heroData.MovementSpeed);

            var interaction = hero.GetComponent<InteractionController>();
            interaction.Init(_interactionService);

            return hero;
        }
    }
}
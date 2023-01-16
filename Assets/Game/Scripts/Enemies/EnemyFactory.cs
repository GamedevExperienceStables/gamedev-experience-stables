using Game.Actors;
using Game.Actors.Health;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Enemies
{
    public class EnemyFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public EnemyFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public EnemyController Create(EnemyDefinition definition, Transform spawnPoint, Transform target, Transform spawnContainer)
        {
            EnemyController instance = _resolver.Instantiate(definition.Prefab, spawnContainer);
            instance.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            instance.SetTarget(target);

            var health = instance.GetComponent<HealthController>();
            health.Init(definition.Health);

            var movement = instance.GetComponent<MovementController>();
            movement.SetMovementSpeed(definition.MovementSpeed);

            return instance;
        }
    }
}
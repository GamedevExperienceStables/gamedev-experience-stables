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

        public EnemyController Create(EnemyDefinition definition, Transform spawnPoint, Transform target,
            Transform spawnContainer)
        {

            EnemyController enemy = Object.Instantiate(definition.Prefab, spawnContainer);
            _resolver.InjectGameObject(enemy.gameObject);

            enemy.InitStats(definition);
            enemy.SetTarget(target);
            enemy.SetLoot(definition.LootBag);
            enemy.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);

            return enemy;
        }
    }
}
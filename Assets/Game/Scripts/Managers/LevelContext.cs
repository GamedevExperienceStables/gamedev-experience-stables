using Game.Hero;
using UnityEngine;

namespace Game.Managers
{
    public class LevelContext : MonoBehaviour
    {
        [SerializeField]
        private Transform heroSpawnPoint;

        [SerializeField]
        private Transform enemiesSpawnerContainer;

        [SerializeField]
        private Transform enemiesContainer;

        public void InitEnemySpawners(Transform target)
        {
            foreach (Transform child in enemiesSpawnerContainer)
            {
                if (child.TryGetComponent(out EnemySpawner spawner))
                {
                    spawner.Init(enemiesContainer);
                    spawner.SetTarget(target);
                }
            }
        }

        public void MoveToSpawnPoint(HeroController hero)
        {
            hero.SetPositionAndRotation(heroSpawnPoint.position, heroSpawnPoint.rotation);
        }

        public void DestroyEnemies()
        {
            foreach (Transform child in enemiesContainer)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
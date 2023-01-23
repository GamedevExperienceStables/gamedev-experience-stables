using Game.RandomManagement;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class LootSpawner
    {
        private readonly LootFactory _factory;
        private readonly RandomService _random;
        private readonly LootSettings _settings;

        [Inject]
        public LootSpawner(LootFactory factory, RandomService random, LootSettings settings)
        {
            _factory = factory;
            _random = random;
            _settings = settings;
        }

        public void SpawnScattered(LootBagDefinition lootBagDefinition, Vector3 position)
        {
            foreach (LootDefinitionItem itemDefinition in lootBagDefinition.Items)
            {
                for (int i = 0; i < itemDefinition.Count; i++)
                {
                    Vector3 spawnPosition = _random.NextRandomInCircle(position, _settings.SpawnScatterRadius);
                    SpawnItem(spawnPosition, itemDefinition);
                }
            }
        }

        private void SpawnItem(Vector3 position, LootDefinitionItem itemDefinition)
        {
            _factory.Create(itemDefinition.Definition, position);
            // toss up item? 
        }
    }
}
using System.Collections.Generic;
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

        private readonly List<LootDefinitionItem> _bufferItems = new();

        [Inject]
        public LootSpawner(LootFactory factory, RandomService random, LootSettings settings)
        {
            _factory = factory;
            _random = random;
            _settings = settings;
        }

        public void SpawnScattered(LootBagDefinition lootBagDefinition, Vector3 position)
        {
            FetchLoot(lootBagDefinition);

            foreach (LootDefinitionItem definition in _bufferItems)
            {
                Vector3 spawnPosition = _random.NextRandomInCircle(position, _settings.SpawnScatterRadius);
                SpawnItem(spawnPosition, definition);
            }

            _bufferItems.Clear();
        }

        private void FetchLoot(LootBagDefinition lootBagDefinition)
        {
            foreach (LootTableDefinitionItem lootTable in lootBagDefinition.Tables)
            {
                lootTable.ComputeWeights();

                if (TryGetLootFromTable(lootTable, out LootDefinitionItem definition))
                    AddItem(definition);
            }

            foreach (LootDefinitionItem definition in lootBagDefinition.Items)
                AddItem(definition);
        }

        private void AddItem(LootDefinitionItem definition)
        {
            for (int i = 0; i < definition.Count; i++)
                _bufferItems.Add(definition);
        }

        private static bool TryGetLootFromTable(LootTableDefinitionItem lootTable, out LootDefinitionItem itemDefinition)
        {
            LootTableItem lootTableItem = lootTable.GetLoot();
            if (lootTableItem.Loot is null)
            {
                itemDefinition = null;
                return false;
            }

            itemDefinition = lootTableItem.Loot;
            return true;
        }

        private void SpawnItem(Vector3 position, LootDefinitionItem item)
        {
            LootItemDefinition lootDefinition = item.Definition;
            _factory.Create(lootDefinition, position);
            // toss up item? 
        }
    }
}
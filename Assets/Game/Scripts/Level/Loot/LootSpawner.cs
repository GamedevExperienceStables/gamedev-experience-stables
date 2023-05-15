using System.Collections.Generic;
using Game.Settings;
using Game.Utils;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class LootSpawner
    {
        private readonly LootFactory _factory;
        private readonly LootSettings _settings;
        private readonly LocationContextHandler _context;

        private readonly List<LootDefinitionItem> _bufferItems = new();

        [Inject]
        public LootSpawner(LootFactory factory, LootSettings settings, LocationContextHandler context)
        {
            _factory = factory;
            _settings = settings;
            _context = context;
        }

        public void SpawnScattered(LootBagDefinition lootBagDefinition, Vector3 position)
        {
            FetchLoot(lootBagDefinition, _bufferItems);

            foreach (LootDefinitionItem definition in _bufferItems)
            {
                Vector3 spawnPosition = GetSpawnPosition(definition, position);
                SpawnItem(definition, spawnPosition);
            }

            _bufferItems.Clear();
        }

        private Vector3 GetSpawnPosition(LootDefinitionItem item, Vector3 position)
        {
            float settingsSpawnScatterRadius = _settings.SpawnScatterRadius;
            
            LootItemDefinition definition = item.Definition;
            if (definition.Overrides.enabled) 
                settingsSpawnScatterRadius = definition.Overrides.spawnScatterRadius;

            Vector3 spawnPosition = RandomUtils.NextRandomInCircle(position, settingsSpawnScatterRadius);
            return spawnPosition;
        }

        public LootItem Spawn(LootItemDefinition definition, Vector3 position)
        {
            LootItem item = _factory.Create(definition, position);
            _context.AddChild(item.gameObject);

            return item;
        }

        private void SpawnItem(LootDefinitionItem item, Vector3 position)
        {
            LootItemDefinition definition = item.Definition;
            LootItem instance = Spawn(definition, position);
            
            // ReSharper disable once InvertIf
            if (definition.Overrides.enabled)
            {
                Transform itemTransform = instance.transform;
                itemTransform.position = itemTransform.TransformDirection(definition.Overrides.offset);
            }
        }

        private static void FetchLoot(LootBagDefinition lootBagDefinition, ICollection<LootDefinitionItem> lootOut)
        {
            foreach (LootTableDefinitionItem lootTable in lootBagDefinition.Tables)
            {
                lootTable.ComputeWeights();

                if (TryGetLootFromTable(lootTable, out LootDefinitionItem definition))
                    AddItem(definition, lootOut);
            }

            foreach (LootDefinitionItem definition in lootBagDefinition.Items)
                AddItem(definition, lootOut);
        }

        private static void AddItem(LootDefinitionItem definition, ICollection<LootDefinitionItem> lootOut)
        {
            for (int i = 0; i < definition.Count; i++)
                lootOut.Add(definition);
        }

        private static bool TryGetLootFromTable(LootTableDefinitionItem lootTable,
            out LootDefinitionItem itemDefinition)
        {
            LootTableItem lootTableItem = lootTable.GetLoot();
            if (lootTableItem.Loot?.Definition)
            {
                itemDefinition = lootTableItem.Loot;
                return true;
            }

            itemDefinition = null;
            return false;
        }
    }
}
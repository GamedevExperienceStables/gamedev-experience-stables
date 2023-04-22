using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class LocationStateHandlerLoot
    {
        private readonly LootSpawner _spawner;
        private readonly LootDataTable _lootDb;

        [Inject]
        public LocationStateHandlerLoot(LootSpawner spawner, LootDataTable lootDb)
        {
            _spawner = spawner;
            _lootDb = lootDb;
        }

        public void Restore(LocationPersistenceLoot locationLoot)
        {
            foreach ((string _, LocationLootData data) in locationLoot)
            {
                if (!_lootDb.Contains(data.definition))
                {
                    Debug.LogWarning($"Loot {data.definition} invalid, ignoring.");
                    continue;
                }

                _spawner.Spawn(data.definition, data.position);
            }
        }

        public void Store(LocationPersistenceLoot locationLoot, ILocationContext context)
        {
            locationLoot.Clear();

            var loot = context.FindAll<ILocationPersistenceLoot>();
            foreach (ILocationPersistenceLoot lootItem in loot)
            {
                if (_lootDb.Contains(lootItem.Value.definition))
                    locationLoot.SetValue(lootItem.Id, lootItem.Value);
            }
        }
    }
}
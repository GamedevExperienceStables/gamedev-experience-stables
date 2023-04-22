using System;
using System.Collections.Generic;
using Game.Level;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class LevelImportExport
    {
        private readonly LevelController _level;
        private readonly LocationDataTable _locationDb;
        private readonly LootDataTable _lootDb;
        private readonly LevelsSettings _settings;
        private readonly ILocationPoint _levelStartPoint;
        private readonly ILocationPoint _loadGameStartPoint;

        [Inject]
        public LevelImportExport(LevelController level, LocationDataTable locationDb, LootDataTable lootDb,
            LevelsSettings settings)
        {
            _level = level;
            _locationDb = locationDb;
            _lootDb = lootDb;
            _settings = settings;
            _levelStartPoint = _settings.LevelStartPoint;
            _loadGameStartPoint = _settings.LoadGameStartPoint;
        }

        public void Reset()
        {
            LevelDefinition firstLevel = _settings.GetFirstLevel();
            _level.InitLevel(firstLevel, _levelStartPoint);
        }

        #region Import

        public void Import(GameSaveData.Level data)
        {
            LevelDefinition currentLevel = GetCurrentLevel(data);
            LevelLocations levelLocations = GetLevelLocations(data);

            _level.InitLevel(currentLevel, _loadGameStartPoint, levelLocations);
        }

        private LevelLocations GetLevelLocations(GameSaveData.Level data)
        {
            var levelLocations = new LevelLocations();

            if (data.locations is null)
            {
                Debug.LogWarning("Save data not contains locations. Skipping");
                return levelLocations;
            }

            foreach (GameSaveData.Location dataLocation in data.locations)
            {
                if (!_locationDb.TryGetValue(dataLocation.id, out LocationDefinition definition))
                {
                    Debug.LogWarning($"Location '{dataLocation.id}' not found. Skipping");
                    continue;
                }

                LocationData location = levelLocations.CreateLocation(definition);
                ImportLocationCounters(dataLocation, location);
                ImportLocationLoot(dataLocation, location);
            }

            return levelLocations;
        }

        private static void ImportLocationCounters(GameSaveData.Location dataLocation, LocationData location)
        {
            if (dataLocation.counters is null)
            {
                Debug.LogWarning("Save data not contains counters. Skipping");
                return;
            }

            foreach (GameSaveData.LocationCounter counter in dataLocation.counters)
                location.Counters.SetValue(counter.id, counter.value);
        }

        private void ImportLocationLoot(GameSaveData.Location dataLocation, LocationData location)
        {
            if (dataLocation.loot is null)
            {
                Debug.LogWarning("Save data not contains loot. Skipping");
                return;
            }

            foreach (GameSaveData.LocationLoot data in dataLocation.loot)
            {
                if (!_lootDb.TryGetValue(data.type, out LootItemDefinition lootDefinition))
                {
                    Debug.LogWarning($"Loot type '{data.type}' not found. Skipping");
                    continue;
                }

                string uid = Guid.NewGuid().ToString();

                var position = new Vector3(data.position[0], data.position[1], data.position[2]);
                var locationLootData = new LocationLootData(lootDefinition, position);

                location.Loot.SetValue(uid, locationLootData);
            }
        }

        private LevelDefinition GetCurrentLevel(GameSaveData.Level data)
        {
            LevelDefinition currentLevel = _settings.FindLevelById(data.id);
            if (currentLevel)
                return currentLevel;

            Debug.LogError($"Level '{data.id}' not found. Used first level");
            return _settings.GetFirstLevel();
        }

        #endregion

        #region Export

        public GameSaveData.Level Export()
        {
            return new GameSaveData.Level
            {
                id = _level.GetCurrentLevelId(),
                locations = ExportLocations()
            };
        }

        private GameSaveData.Location[] ExportLocations()
        {
            var locations = new List<GameSaveData.Location>();
            foreach (LocationData data in _level.GetLocations())
            {
                GameSaveData.Location location = ExportLocation(data);
                locations.Add(location);
            }

            return locations.ToArray();
        }

        private static GameSaveData.Location ExportLocation(LocationData locationData) =>
            new()
            {
                id = locationData.Definition.Id,
                counters = ExportLocationCounters(locationData),
                loot = ExportLocationLoot(locationData)
            };

        private static GameSaveData.LocationCounter[] ExportLocationCounters(LocationData data)
        {
            var counters = new List<GameSaveData.LocationCounter>();
            foreach ((string id, int value) in data.Counters)
            {
                counters.Add(new GameSaveData.LocationCounter
                {
                    id = id,
                    value = value
                });
            }
            return counters.ToArray();
        }

        private static GameSaveData.LocationLoot[] ExportLocationLoot(LocationData data)
        {
            var loot = new List<GameSaveData.LocationLoot>();
            foreach ((string _, LocationLootData value) in data.Loot)
            {
                float[] serializedPosition = { value.position.x, value.position.y, value.position.z };
                loot.Add(new GameSaveData.LocationLoot
                {
                    type = value.definition.Id,
                    position = serializedPosition
                });
            }

            return loot.ToArray();
        }

        #endregion
    }
}
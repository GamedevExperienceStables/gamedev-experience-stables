using System.Collections.Generic;
using System.Linq;
using Game.Hero;
using Game.Inventory;
using Game.Level;
using Game.Player;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class PlayerImportExport
    {
        private readonly HeroStats.InitialStats _initialStats;
        private readonly PlayerController _player;
        private readonly InventoryController _inventory;
        private readonly LevelController _level;
        private readonly LevelsSettings _levelsSettings;

        private readonly RecipeDataTable _recipesDb;
        private readonly RuneDataTable _runesDb;

        [Inject]
        public PlayerImportExport(
            HeroStats.InitialStats initialStats,
            PlayerController player,
            InventoryController inventory,
            RecipeDataTable recipeDb,
            RuneDataTable runesDb,
            LevelController level,
            LevelsSettings levelsSettings
        )
        {
            _initialStats = initialStats;
            _player = player;
            _inventory = inventory;
            _recipesDb = recipeDb;
            _runesDb = runesDb;
            _level = level;
            _levelsSettings = levelsSettings;
        }

        public void Reset()
        {
            _player.Init(_initialStats);
            _inventory.Reset();
        }

        public GameSaveData.Player Export()
        {
            var recipes = _inventory.Recipes.Items;
            string[] recipeIds = recipes.Select(definition => definition.Id).ToArray();

            var slots = _inventory.Slots.Items;
            string[] slotRuneIds = slots.Values
                .Select(slot => slot.IsEmpty ? string.Empty : slot.Rune.Id)
                .ToArray();

            MaterialDefinition levelMaterial = _level.GetCurrentLevel().Goal.Material;
            IReadOnlyMaterials materials = _inventory.Materials;

            return new GameSaveData.Player
            {
                recipes = recipeIds,
                slots = slotRuneIds,
                containerMaterials = materials.Container.GetCurrentValue(levelMaterial),
                bagMaterials = materials.Bag.GetCurrentValue(levelMaterial),
            };
        }

        public void Import(GameSaveData.Player data)
        {
            _player.Init(_initialStats);

            LevelDefinition currentLevel = _level.GetCurrentLevel();

            var recipes = ImportRecipes(data.recipes);
            var slots = ImportSlots(data.slots, recipes);

            var inventoryData = new InventoryInitialData
            {
                bag = ImportBag(data.bagMaterials, currentLevel),
                container = ImportContainer(data.containerMaterials, currentLevel),
                recipes = recipes,
                slots = slots,
            };

            _inventory.Init(inventoryData);
        }

        private IDictionary<RuneSlotId, RuneDefinition> ImportSlots(
            IList<string> runeIds,
            IList<RecipeDefinition> recipes
        )
        {
            Dictionary<RuneSlotId, RuneDefinition> slots = new();

            if (runeIds is null)
                return slots;

            for (int index = 0; index < runeIds.Count; index++)
            {
                int slotId = index + 1;

                string runeId = runeIds[index];
                if (string.IsNullOrEmpty(runeId))
                {
                    slots[slotId] = default;
                    continue;
                }

                if (!_runesDb.TryGetValue(runeId, out RuneDefinition rune))
                {
                    Debug.LogError($"Save data corrupted! Not found rune with id: {runeId}");
                    continue;
                }

                // validate the player has rune
                if (recipes.Any(x => x.GrantsRune == rune))
                {
                    Debug.LogError($"Trying add rune {rune.name} that player not obtained.");
                    continue;
                }

                slots[slotId] = rune;
            }

            return slots;
        }

        private IList<RecipeDefinition> ImportRecipes(IEnumerable<string> recipeIds)
        {
            List<RecipeDefinition> obtainedRecipes = new();
            foreach (string recipeId in recipeIds)
            {
                if (!_recipesDb.TryGetValue(recipeId, out RecipeDefinition recipe))
                {
                    Debug.LogError($"Save data corrupted! Not found recipe with id: {recipeId}");
                    continue;
                }

                obtainedRecipes.Add(recipe);
            }

            return obtainedRecipes;
        }

        private static IList<MaterialInitialData> ImportBag(int quantity, LevelDefinition currentLevel)
        {
            return new List<MaterialInitialData>
            {
                new()
                {
                    material = currentLevel.Goal.Material,
                    quantity = quantity
                }
            };
        }

        private IList<MaterialInitialData> ImportContainer(int quantity, LevelDefinition currentLevel)
        {
            List<MaterialInitialData> materials = new();

            int currentLevelIndex = _levelsSettings.Levels.IndexOf(currentLevel);

            int total = _levelsSettings.Levels.Count;
            for (int index = 0; index < total; index++)
            {
                LevelGoalSettings levelGoal = _levelsSettings.Levels[index].Goal;

                int levelQuantity = GetLevelMaterialQuantity(index, currentLevelIndex, quantity, levelGoal.Count);
                materials.Add(new MaterialInitialData
                {
                    material = levelGoal.Material,
                    quantity = levelQuantity
                });
            }

            return materials;
        }

        private static int GetLevelMaterialQuantity(int index, int currentLevelIndex, int currentQuantity,
            int levelGoalCount)
        {
            if (index < currentLevelIndex)
                return levelGoalCount;

            if (index == currentLevelIndex)
                return currentQuantity;

            return 0;
        }
    }
}
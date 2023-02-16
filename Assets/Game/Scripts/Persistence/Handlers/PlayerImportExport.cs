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
        private readonly RecipeDataTable _recipesDataTable;
        private readonly LevelController _level;
        private readonly LevelsSettings _levelsSettings;

        [Inject]
        public PlayerImportExport(
            HeroStats.InitialStats initialStats,
            PlayerController player,
            InventoryController inventory,
            RecipeDataTable recipeDataTable,
            LevelController level,
            LevelsSettings levelsSettings
        )
        {
            _initialStats = initialStats;
            _player = player;
            _inventory = inventory;
            _recipesDataTable = recipeDataTable;
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

            MaterialDefinition planetMaterial = _level.GetCurrentLevel().Goal.Material;
            IReadOnlyMaterials materials = _inventory.Materials;

            return new GameSaveData.Player
            {
                recipes = recipeIds,
                containerMaterials = materials.Container.GetCurrentValue(planetMaterial),
                bagMaterials = materials.Bag.GetCurrentValue(planetMaterial)
            };
        }

        public void Import(GameSaveData.Player data)
        {
            _player.Init(_initialStats);

            LevelDefinition currentLevel = _level.GetCurrentLevel();

            var inventoryData = new InventoryInitialData
            {
                bag = ImportBag(data.bagMaterials, currentLevel),
                container = ImportContainer(data.containerMaterials, currentLevel),
                recipes = ImportRecipes(data.recipes),
            };

            _inventory.Init(inventoryData);
        }

        private IList<RecipeDefinition> ImportRecipes(IEnumerable<string> recipeIds)
        {
            List<RecipeDefinition> obtainedRecipes = new();
            foreach (string recipeId in recipeIds)
            {
                if (!_recipesDataTable.TryGetValue(recipeId, out RecipeDefinition recipe))
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
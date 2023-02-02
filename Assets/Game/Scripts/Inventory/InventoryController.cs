using System;
using Game.Actors;
using Game.Settings;
using UnityEngine;
using VContainer;

namespace Game.Inventory
{
    public class InventoryController : IInventory
    {
        private readonly Materials _materials;
        private readonly Recipes _recipes;
        private readonly Runes _runes;

        [Inject]
        public InventoryController(Settings settings, LevelsSettings levels, InventoryData data)
        {
            _materials = data.Materials;
            _recipes = data.Recipes;
            _runes = data.Runes;

            _materials.CreateDefaults(levels, settings.BagMaxStack);
        }

        public IReadOnlyMaterials Materials => _materials;
        public IReadOnlyRecipes Recipes => _recipes;
        public IReadOnlyRunes Runes => _runes;

        public void Init()
        {
            _materials.Init();
            _recipes.Init();
            _runes.Init();
        }

        public void Init(InventoryInitialData data)
        {
            _runes.Init();
            
            _materials.Init(data.container, data.bag);
            _recipes.Init(data.recipes);
        }

        public bool CanTransferToContainer(MaterialDefinition definition)
            => _materials.CanTransferToContainer(definition);

        public void TransferToContainer(MaterialDefinition definition)
            => _materials.TransferToContainer(definition);

        public bool CanAddToBag(ItemDefinition definition, IActorController owner)
            => definition switch
            {
                IItemExecutableDefinition executableItem => executableItem.CanExecute(
                    new ItemExecutionContext(owner, this)),
                MaterialDefinition material => _materials.CanAddToBag(material),
                _ => false
            };

        public bool TryAddToBag(ItemDefinition item, IActorController owner)
        {
            if (!CanAddToBag(item, owner))
                return false;

            AddToBag(item, owner);
            return true;
        }

        public void AddToBag(ItemDefinition item, IActorController owner)
        {
            if (item is IItemExecutableDefinition executableItem)
                executableItem.Execute(new ItemExecutionContext(owner, this));

            switch (item)
            {
                case MaterialDefinition material:
                    _materials.AddToBag(material);
                    break;

                case RecipeDefinition recipe:
                    _recipes.Add(recipe);
                    break;

                case RuneDefinition rune:
                    _runes.Add(rune);
                    break;
            }
        }

        [Serializable]
        public class Settings
        {
            [SerializeField, Min(0)]
            private int bagMaxStack = 10;

            public int BagMaxStack => bagMaxStack;
        }
    }
}
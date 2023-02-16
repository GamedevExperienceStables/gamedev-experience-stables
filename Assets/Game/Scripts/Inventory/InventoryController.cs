using Game.Actors;
using VContainer;

namespace Game.Inventory
{
    public class InventoryController : IInventory
    {
        private readonly Materials _materials;
        private readonly Recipes _recipes;
        private readonly Runes _runes;

        [Inject]
        public InventoryController(InventoryData data)
        {
            _materials = data.Materials;
            _recipes = data.Recipes;
            _runes = data.Runes;
        }

        public IReadOnlyMaterials Materials => _materials;
        public IReadOnlyRecipes Recipes => _recipes;
        public IReadOnlyRunes Runes => _runes;

        public void Reset()
        {
            _materials.Reset();
            _recipes.Reset();
            _runes.Reset();
        }

        public void Init(InventoryInitialData data)
        {
            _runes.Reset();
            
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
        
        public void ClearBag() 
            => _materials.ClearBag();

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
    }
}
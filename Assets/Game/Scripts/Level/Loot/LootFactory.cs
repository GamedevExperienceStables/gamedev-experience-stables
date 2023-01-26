using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Level
{
    public class LootFactory
    {
        private readonly IObjectResolver _resolver;

        [Inject]
        public LootFactory(IObjectResolver resolver)
            => _resolver = resolver;

        public LootItem Create(LootItemDefinition itemDefinition, Vector3 position)
        {
            LootItem item = _resolver.Instantiate(itemDefinition.Prefab, position, Quaternion.identity);
            item.Init(itemDefinition.ItemDefinition);
            
            return item;
        }
    }
}
using UnityEngine;

namespace Game.Level
{
    public class LootItem : MonoBehaviour
    {
        public LootItemDefinition Definition { get; private set; }

        public void Init(LootItemDefinition lootDefinition) 
            => Definition = lootDefinition;
    }
}
using UnityEngine;

namespace Game.Level
{
    public class LootItem : MonoBehaviour
    {
        [SerializeField]
        private LootItemDefinition definition;

        public LootItemDefinition Definition => definition;

        public void Init(LootItemDefinition lootDefinition) 
            => definition = lootDefinition;
    }
}
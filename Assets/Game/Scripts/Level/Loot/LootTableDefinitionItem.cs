using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Loot/Loot Table")]
    public class LootTableDefinitionItem : ScriptableObject
    {
        [SerializeField]
        private LootTable table;

        protected void OnValidate()
            => ComputeWeights();

        public void ComputeWeights()
            => table.ComputeWeights();

        public LootTableItem GetLoot()
            => table.GetLoot();
    }
}
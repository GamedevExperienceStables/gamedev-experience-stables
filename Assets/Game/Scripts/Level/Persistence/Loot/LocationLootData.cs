using UnityEngine;

namespace Game.Level
{
    public readonly struct LocationLootData
    {
        public readonly LootItemDefinition definition;
        public readonly Vector3 position;

        public LocationLootData(LootItemDefinition definition, Vector3 position)
        {
            this.definition = definition;
            this.position = position;
        }
    }
}
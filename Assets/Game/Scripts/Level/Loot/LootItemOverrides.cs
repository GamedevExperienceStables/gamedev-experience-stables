using System;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public struct LootItemOverrides
    {
        public bool enabled;

        [EnableIf(nameof(enabled)), AllowNesting]
        [Range(0, 10)]
        public float spawnScatterRadius;
        
        [EnableIf(nameof(enabled)), AllowNesting]
        public Vector3 offset;
    }
}
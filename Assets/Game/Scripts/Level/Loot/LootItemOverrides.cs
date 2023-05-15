using System;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public struct LootItemOverrides
    {
        public bool enabled;
        
        [Range(0, 10), ShowIf(nameof(enabled)), AllowNesting]
        public float spawnScatterRadius;
        
        [ShowIf(nameof(enabled)), AllowNesting]
        public Vector3 offset;
    }
}
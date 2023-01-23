using System;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class LootSettings
    {
        [SerializeField, Min(0)]
        private float spawnScatterRadius = 2f;

        public float SpawnScatterRadius => spawnScatterRadius;
    }
}
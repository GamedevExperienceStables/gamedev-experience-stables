using System;
using UnityEngine;

namespace Game.Level
{
    [Serializable]
    public class PoolSettings
    {
        [Min(0)]
        public int prewarmCount;

        [Min(5)]
        public int maxPoolSize = 20;

        public bool clearOnEnterLocation;
    }
}
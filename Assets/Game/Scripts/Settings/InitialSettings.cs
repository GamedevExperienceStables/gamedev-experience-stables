using System;
using Game.Level;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class InitialSettings
    {
        [SerializeField]
        private LocationPointDefinition startPoint;

        public LocationPointDefinition StartPoint => startPoint;
    }
}
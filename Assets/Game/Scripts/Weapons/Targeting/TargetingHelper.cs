using System;
using UnityEngine;

namespace Game.Weapons
{
    [Serializable]
    public class TargetingHelper
    {
        [Min(0)]
        public float radius = 1.5f;
            
        [Range(0f, 1f)]    
        public float power = 0.5f;
        
        public LayerMask layerMask;
    }
}
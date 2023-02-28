using System;
using UnityEngine;

namespace Game.Enemies
{
    [Serializable]
    public class AttackSettings
    {
        [SerializeField]
        private float range = 8f;
        [SerializeField]
        private float interval = 0.8f;

        public float Range => range;
            
        public float Interval => interval;
    }
}
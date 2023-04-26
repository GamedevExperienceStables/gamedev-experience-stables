using System;
using UnityEngine;

namespace Game.Actors
{
    [Serializable]
    public class GroundMovement
    {
        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        private float sharpness = 15f;
        
        public float MaxSpeed => maxSpeed;
        public float Sharpness => sharpness;
    }
}
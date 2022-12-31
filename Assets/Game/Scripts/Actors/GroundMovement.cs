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

        private float _initialMaxSpeed;

        public float MaxSpeed => maxSpeed;
        public float Sharpness => sharpness;

        public void Init()
        {
            _initialMaxSpeed = MaxSpeed;
        }

        public void SetMaxSpeed(float newMaxSpeed)
        {
            maxSpeed = newMaxSpeed;
        }

        public void ResetMaxSpeed()
        {
            maxSpeed = _initialMaxSpeed;
        }
    }
}
using System;

namespace Game.Actors
{
    [Serializable]
    public class AirMovement
    {
        public float maxSpeed = 10f;
        public float acceleration = 5f;
        public float drag = 0.1f;
    }
}
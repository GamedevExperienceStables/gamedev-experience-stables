using System;

namespace Game.Actors
{
    [Serializable]
    public class AirMovement
    {
        public float maxSpeed = 20f;
        public float acceleration = 1f;
        public float drag = 1f;
    }
}
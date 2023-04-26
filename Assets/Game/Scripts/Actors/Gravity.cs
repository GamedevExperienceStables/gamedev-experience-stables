using System;
using UnityEngine;

namespace Game.Actors
{
    [Serializable]
    public class Gravity
    {
        public Vector3 direction = new(0, -30f, 0);
    }
}
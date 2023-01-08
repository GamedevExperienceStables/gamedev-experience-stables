﻿using UnityEngine;

namespace Game.Utils
{
    public static class MathExtensions
    {
        public const float EPSILON = 0.0001f;
        public const float EPSILON_SQR = EPSILON * EPSILON;

        public static bool AlmostZero(this float v)
        {
            return Mathf.Abs(v) < EPSILON;
        }

        public static bool AlmostEquals(float a, float b)
        {
            return AlmostZero(a - b);
        }
    }
}
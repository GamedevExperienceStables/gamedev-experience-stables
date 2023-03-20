using UnityEngine;

namespace Game.Utils
{
    public static class MathExtensions
    {
        public const float EPSILON = 0.0001f;

        public static bool AlmostZero(this float v)
        {
            return Mathf.Abs(v) < EPSILON;
        }

        public static bool AlmostEquals(this float a, float b)
        {
            return AlmostZero(a - b);
        }
        
        public static bool AlmostEquals(this Vector3 a, Vector3 b)
        {
            return AlmostEquals(a.x, b.x) && AlmostEquals(a.y, b.y) && AlmostEquals(a.z, b.z);
        }

        public static float Remap(float input, float inputMin, float inputMax, float min, float max)
        {
            return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
        }
    }
}
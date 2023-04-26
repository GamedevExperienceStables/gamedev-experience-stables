using UnityEngine;

namespace Game.Utils
{
    public static class MathExtensions
    {
        public const float EPSILON = 0.0001f;

        public static bool AlmostZero(this float v, float threshold = EPSILON)
        {
            return Mathf.Abs(v) < threshold;
        }

        public static bool AlmostEquals(this float a, float b, float threshold = EPSILON)
        {
            return AlmostZero(a - b, threshold);
        }

        public static bool AlmostEquals(this Vector3 a, Vector3 b, float threshold = EPSILON)
        {
            return AlmostEquals(a.x, b.x, threshold) &&
                   AlmostEquals(a.y, b.y, threshold) &&
                   AlmostEquals(a.z, b.z, threshold);
        }

        public static float Remap(float input, float inputMin, float inputMax, float min, float max)
        {
            return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
        }
        
        public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Quaternion rotation, float smoothTime) {
            float dot = Quaternion.Dot(current, target);
            float multi = dot > 0f ? 1f : -1f;
            
            target.x *= multi;
            target.y *= multi;
            target.z *= multi;
            target.w *= multi;
            
            
            Vector4 result = new Vector4(
                Mathf.SmoothDamp(current.x, target.x, ref rotation.x, smoothTime),
                Mathf.SmoothDamp(current.y, target.y, ref rotation.y, smoothTime),
                Mathf.SmoothDamp(current.z, target.z, ref rotation.z, smoothTime),
                Mathf.SmoothDamp(current.w, target.w, ref rotation.w, smoothTime)
            ).normalized;

            Vector4 derivError = Vector4.Project(new Vector4(rotation.x, rotation.y, rotation.z, rotation.w), result);
            rotation.x -= derivError.x;
            rotation.y -= derivError.y;
            rotation.z -= derivError.z;
            rotation.w -= derivError.w;		
		
            return new Quaternion(result.x, result.y, result.z, result.w);
        }

        public static float AddPercent(this float baseValue, float value)
        {
            float additiveValue = baseValue * value;
            return baseValue + additiveValue;
        }
    }
}
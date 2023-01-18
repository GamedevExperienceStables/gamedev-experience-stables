using JetBrains.Annotations;
using UnityEngine;

namespace Game.RandomManagement
{
    [UsedImplicitly]
    public class RandomService
    {
        public Vector3 NextRandomInCircle(Vector3 position, float radius)
        {
            Vector2 value = GetRandomInsideUnitCircle() * radius;
            return position + new Vector3(value.x, 0, value.y);
        }

        private static Vector2 GetRandomInsideUnitCircle() 
            => Random.insideUnitCircle;
    }
}
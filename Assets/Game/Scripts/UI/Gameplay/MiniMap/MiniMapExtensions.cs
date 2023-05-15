using Game.Utils;
using UnityEngine;

namespace Game.UI
{
    public static class MiniMapExtensions
    {
        private const int WIDTH = 1600;

        public static void GetMapData(Sprite image, out Bounds mapBounds, out Rect mapRect)
        {
            float ratio = image.rect.width / WIDTH;
            mapRect = new Rect(image.rect.x, image.rect.y, WIDTH, image.rect.height / ratio);
            mapBounds = new Bounds(mapRect.center, mapRect.size);
        }

        public static Vector2 WorldToMapPosition(Vector3 position, Bounds worldBounds, Bounds mapBounds)
        {
            Vector3 worldMin = worldBounds.min;
            Vector3 worldMax = worldBounds.max;

            Vector3 mapMin = mapBounds.min;
            Vector3 mapMax = mapBounds.max;

            float x = MathExtensions.Remap(position.x, worldMin.x, worldMax.x, mapMin.x, mapMax.x);

            // should be used 'max' value as 'min', because origin of the map coordinates
            // goes from top to bottom, and the world coordinates from bottom to top 
            float y = MathExtensions.Remap(position.z, worldMin.z, worldMax.z, mapMax.y, mapMin.y);

            return new Vector2(x, y);
        }

        public static Vector3 MapToWorldPosition(Vector2 position, Bounds worldBounds, Bounds mapBounds)
        {
            Vector3 worldMin = worldBounds.min;
            Vector3 worldMax = worldBounds.max;

            Vector3 mapMin = mapBounds.min;
            Vector3 mapMax = mapBounds.max;

            float x = MathExtensions.Remap(position.x, mapMin.x, mapMax.x, worldMin.x, worldMax.x);


            float y = MathExtensions.Remap(position.y, mapMin.y, mapMax.y, worldMax.z, worldMin.z);

            return new Vector3(x, 0, y);
        }
    }
}
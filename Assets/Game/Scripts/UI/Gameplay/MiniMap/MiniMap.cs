using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class MiniMap
    {
        private readonly VisualElement _map;

        private Bounds _worldBounds;
        private Bounds _mapBounds;

        public MiniMap(VisualElement map)
        {
            _map = map;
        }

        public void Init(Sprite image, Bounds worldBounds)
        {
            _worldBounds = worldBounds;

            Rect mapRect = image.rect;
            _mapBounds = new Bounds(mapRect.center, mapRect.size);

            SetImage(image, mapRect);
        }

        public Vector2 WorldToMapPosition(Vector3 position)
        {
            Vector3 worldMin = _worldBounds.min;
            Vector3 worldMax = _worldBounds.max;

            Vector3 min = _mapBounds.min;
            Vector3 max = _mapBounds.max;

            float x = MathExtensions.Remap(position.x, worldMin.x, worldMax.x, min.x, max.x);

            // should be used 'max' value as 'min', because origin of the map coordinates
            // goes from top to bottom, and the world coordinates from bottom to top 
            float y = MathExtensions.Remap(position.z, worldMin.z, worldMax.z, max.y, min.y);

            return new Vector2(x, y);
        }

        public void SetCenterPosition(Vector2 position)
            => _map.style.translate = new Translate(-position.x, -position.y);

        public void Add(VisualElement element)
            => _map.Add(element);

        public void Remove(VisualElement element)
            => _map.Remove(element);

        public void Clear()
        {
            _map.style.backgroundImage = null;
            _map.style.translate = default;
        }

        private void SetImage(Sprite image, Rect mapRect)
        {
            _map.style.backgroundImage = new StyleBackground(image);
            _map.style.width = mapRect.width;
            _map.style.height = mapRect.height;
        }
    }
}
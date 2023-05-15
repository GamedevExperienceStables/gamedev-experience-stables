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
            
            MiniMapExtensions.GetMapData(image,out _mapBounds, out Rect mapRect);

            SetImage(image, mapRect);
        }

        public Vector2 WorldToMapPosition(Vector3 position) 
            => MiniMapExtensions.WorldToMapPosition(position, _worldBounds, _mapBounds);

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
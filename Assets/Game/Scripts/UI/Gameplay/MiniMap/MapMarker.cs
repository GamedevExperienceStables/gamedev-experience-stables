using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class MapMarker
    {
        private readonly ILocationMarker _owner;
        private Vector3 _lastWorldPosition;

        public MapMarker(ILocationMarker owner, VisualElement element)
        {
            _owner = owner;
            Element = element;
        }

        public Vector3 WorldPosition => _owner.Position;

        public VisualElement Element { get; }

        public void UpdatePosition(Vector2 value)
            => Element.style.translate = new Translate(value.x, value.y);

        public void UpdateRotation(float mapAngle) 
            => Element.style.rotate = new Rotate(mapAngle);

        public bool IsWorldPositionChanged()
        {
            if (_lastWorldPosition.AlmostEquals(_owner.Position))
                return false;

            _lastWorldPosition = _owner.Position;
            return true;
        }

        public bool Is(ILocationMarker target)
            => _owner == target;
    }
}
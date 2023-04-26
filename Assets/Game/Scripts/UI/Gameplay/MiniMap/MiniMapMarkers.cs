using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class MiniMapMarkers
    {
        private readonly List<MapMarker> _markers = new();
        private readonly MiniMap _map;

        public MiniMapMarkers(MiniMap map)
            => _map = map;

        public void Create(ILocationMarker locationMarker)
        {
            MapMarker marker = CreateMarker(locationMarker);

            _map.Add(marker.Element);
            _markers.Add(marker);

            // move marker under hero marker
            marker.Element.SendToBack();
        }

        public void Remove(ILocationMarker locationMarker)
        {
            MapMarker marker = _markers.Find(m => m.Is(locationMarker));
            if (marker is null)
                return;

            _map.Remove(marker.Element);
            _markers.Remove(marker);
        }

        public void UpdatePosition()
        {
            for (int i = _markers.Count - 1; i >= 0; i--) 
                UpdateMarkerPosition(_markers[i]);
        }

        public void UpdateRotation(float angle)
        {
            for (int i = _markers.Count - 1; i >= 0; i--) 
                _markers[i].UpdateRotation(angle);
        }

        private static MapMarker CreateMarker(ILocationMarker target)
        {
            var icon = new Image { sprite = target.Icon };
            icon.AddToClassList(LayoutNames.MiniMap.MARKER_CLASS_NAME);

            var marker = new MapMarker(target, icon);
            return marker;
        }

        private void UpdateMarkerPosition(MapMarker marker)
        {
            if (!marker.IsWorldPositionChanged())
                return;

            Vector2 mapPosition = _map.WorldToMapPosition(marker.WorldPosition);
            marker.UpdatePosition(mapPosition);
        }

        public void Clear()
        {
            foreach (MapMarker marker in _markers)
                _map.Remove(marker.Element);

            _markers.Clear();
        }
    }
}
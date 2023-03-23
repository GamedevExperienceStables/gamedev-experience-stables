using System;
using Game.Cameras;
using Game.Level;
using UnityEngine;
using VContainer;

namespace Game.UI
{
    public class MiniMapViewModel
    {
        private readonly LocationController _location;
        private readonly LocationMarkers _markers;
        private readonly SceneCamera _sceneCamera;

        [Inject]
        public MiniMapViewModel(LocationController location, LocationMarkers markers, SceneCamera sceneCamera)
        {
            _location = location;
            _markers = markers;
            _sceneCamera = sceneCamera;
        }

        public void SubscribeLocationInitialized(Action callback)
            => _location.Initialized += callback;

        public void UnSubscribeLocationInitialized(Action callback)
            => _location.Initialized -= callback;

        public Transform Hero => _location.Hero;
        public Transform LocationCamera => _sceneCamera.transform;

        public Bounds LocationBounds => _location.Bounds;
        public ILocationDefinition LocationDefinition => _location.LocationDefinition;

        public void SubscribeMarkerAdd(Action<ILocationMarker> callback)
            => _markers.Added += callback;

        public void UnSubscribeMarkerAdd(Action<ILocationMarker> callback)
            => _markers.Added -= callback;

        public void SubscribeMarkerRemove(Action<ILocationMarker> callback)
            => _markers.Removed += callback;

        public void UnSubscribeMarkerRemove(Action<ILocationMarker> callback)
            => _markers.Removed -= callback;
    }
}
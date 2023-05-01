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
        private readonly ILocationContext _context;
        private readonly LocationMarkers _markers;
        private readonly SceneCamera _sceneCamera;

        [Inject]
        public MiniMapViewModel(LocationController location, ILocationContext context, LocationMarkers markers,
            SceneCamera sceneCamera)
        {
            _location = location;
            _context = context;
            _markers = markers;
            _sceneCamera = sceneCamera;
        }

        public void SubscribeLocationInitialized(Action callback)
            => _location.Initialized += callback;

        public void UnSubscribeLocationInitialized(Action callback)
            => _location.Initialized -= callback;

        public Transform Hero => _location.Hero;
        public Transform LocationCamera => _sceneCamera.transform;

        public Bounds LocationBounds => _context.LocationBounds.Bounds;
        public ILocationDefinition LocationDefinition => _context.Location;

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
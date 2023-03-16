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
        private readonly SceneCamera _sceneCamera;

        [Inject]
        public MiniMapViewModel(LocationController location, SceneCamera sceneCamera)
        {
            _location = location;
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
    }
}
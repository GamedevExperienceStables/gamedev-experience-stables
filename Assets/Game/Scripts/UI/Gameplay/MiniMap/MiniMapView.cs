using Game.Level;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class MiniMapView
    {
        private readonly MiniMapViewModel _viewModel;
        private VisualElement _playerMarker;
        private VisualElement _root;
        private VisualElement _mapWrapper;

        private bool _isActive;

        private Transform _hero;

        private MiniMap _miniMap;
        private MiniMapMarkers _miniMapMarkers;
        private Label _miniMapCoordinates;

        private float _lastCameraAngle;


        [Inject]
        public MiniMapView(MiniMapViewModel viewModel)
            => _viewModel = viewModel;

        public void Create(VisualElement root)
        {
            CreateMap(root);

            _viewModel.SubscribeLocationInitialized(OnLocationInitialized);

            _viewModel.SubscribeMarkerAdd(OnMarkerAdd);
            _viewModel.SubscribeMarkerRemove(OnMarkerRemove);
        }

        public void Destroy()
        {
            _viewModel.UnSubscribeLocationInitialized(OnLocationInitialized);

            _viewModel.UnSubscribeMarkerAdd(OnMarkerAdd);
            _viewModel.UnSubscribeMarkerRemove(OnMarkerRemove);
        }

        private void CreateMap(VisualElement root)
        {
            _root = root.Q<VisualElement>(LayoutNames.MiniMap.ROOT);
            _mapWrapper = _root.Q<VisualElement>(LayoutNames.MiniMap.MAP_WRAPPER);

            _playerMarker = _root.Q<VisualElement>(LayoutNames.MiniMap.PLAYER);

            var map = _root.Q<VisualElement>(LayoutNames.MiniMap.MAP);

            _miniMap = new MiniMap(map);
            _miniMapMarkers = new MiniMapMarkers(_miniMap);
            _miniMapCoordinates = _root.Q<Label>(LayoutNames.MiniMap.MAP_COORDINATES);
        }

        private void OnMarkerAdd(ILocationMarker target)
            => _miniMapMarkers.Create(target);

        private void OnMarkerRemove(ILocationMarker target)
            => _miniMapMarkers.Remove(target);

        private void OnLocationInitialized()
        {
            Reset();
            
            ILocationDefinition definition = _viewModel.LocationDefinition;
            if (!MapExists(definition))
            {
                Hide();
                return;
            }

            _miniMap.Init(definition.MapImage, _viewModel.LocationBounds);
            _hero = _viewModel.Hero;

            Show();
        }

        public void LateTick()
        {
            if (!_isActive)
                return;

            float cameraAngle = _viewModel.LocationCamera.eulerAngles.y;

            Vector2 heroMapPosition = UpdateHeroMarker();    
            _miniMap.SetCenterPosition(heroMapPosition);
            
            UpdateMapCoordinates(heroMapPosition);
            UpdateMapRotation(cameraAngle);
            UpdateMarkers(cameraAngle);

            _lastCameraAngle = cameraAngle;
        }

        private void UpdateMarkers(float cameraAngle)
        {
            _miniMapMarkers.UpdatePosition();
            
            if (!_lastCameraAngle.AlmostEquals(cameraAngle))
                _miniMapMarkers.UpdateRotation(-cameraAngle);
        }

        private void UpdateMapCoordinates(Vector2 position) 
            => _miniMapCoordinates.text = $"{position.x:f0} : {position.y:f0}";

        private Vector2 UpdateHeroMarker()
        {
            Vector2 heroMapPosition = _miniMap.WorldToMapPosition(_hero.position);
            _playerMarker.style.translate = new Translate(heroMapPosition.x, heroMapPosition.y);
            _playerMarker.style.rotate = new Rotate(_hero.eulerAngles.y);

            return heroMapPosition;
        }

        private void UpdateMapRotation(float angle)
            => _mapWrapper.style.rotate = new Rotate(angle);

        private void Reset()
        {
            _hero = null;

            _miniMap.Clear();
            _miniMapMarkers.Clear();

            _lastCameraAngle = 0;
            _mapWrapper.style.rotate = default;
        }

        private void Hide()
        {
            _isActive = false;
            _root.AddToClassList(LayoutNames.MiniMap.MAP_HIDDEN_CLASS_NAME);
        }

        private void Show()
        {
            _isActive = true;
            _root.RemoveFromClassList(LayoutNames.MiniMap.MAP_HIDDEN_CLASS_NAME);
        }

        private static bool MapExists(ILocationDefinition definition)
            => definition.MapImage;
    }
}
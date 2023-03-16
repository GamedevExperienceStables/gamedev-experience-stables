using Game.Level;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Game.UI
{
    public class MiniMapView : ILateTickable
    {
        private struct MiniMapData
        {
            public Bounds mapBounds;
            public Bounds worldBounds;
        }

        private readonly MiniMapViewModel _viewModel;
        private VisualElement _playerRepresentation;
        private VisualElement _container;
        private VisualElement _mapWrapper;
        private VisualElement _map;

        private bool _isActive;

        private MiniMapData _miniMap;
        private Transform _hero;

        [Inject]
        public MiniMapView(MiniMapViewModel viewModel)
            => _viewModel = viewModel;

        public void Create(VisualElement root)
        {
            CreateMap(root);

            _viewModel.SubscribeLocationInitialized(OnLocationInitialized);
        }

        public void Destroy()
            => _viewModel.UnSubscribeLocationInitialized(OnLocationInitialized);

        private void CreateMap(VisualElement root)
        {
            _container = root.Q<VisualElement>("mini-map");

            _mapWrapper = _container.Q<VisualElement>("map-wrapper");
            _map = _container.Q<VisualElement>("map");

            _playerRepresentation = _container.Q<VisualElement>("player");
        }

        private void OnLocationInitialized()
        {
            ILocationDefinition definition = _viewModel.LocationDefinition;
            if (!MapExists(definition))
            {
                Reset();
                Hide();
                return;
            }

            Rect mapRect = definition.MapImage.rect;

            _miniMap = new MiniMapData
            {
                mapBounds = new Bounds(mapRect.center, mapRect.size),
                worldBounds = _viewModel.LocationBounds,
            };

            _map.style.backgroundImage = new StyleBackground(definition.MapImage);
            _map.style.width = mapRect.width;
            _map.style.height = mapRect.height;

            _hero = _viewModel.Hero;

            Show();
        }

        public void LateTick()
        {
            if (!_isActive)
                return;

            Vector2 heroMapPosition = UpdateHeroMarker();

            UpdateCenterPosition(heroMapPosition);
            UpdateMapRotation();
        }

        private Vector2 UpdateHeroMarker()
        {
            Vector2 heroMapPosition = WorldToMapPosition(_hero.position);
            _playerRepresentation.style.translate = new Translate(heroMapPosition.x, heroMapPosition.y);

            return heroMapPosition;
        }

        private void UpdateMapRotation()
        {
            float angle = _viewModel.LocationCamera.eulerAngles.y;
            _mapWrapper.style.rotate = new Rotate(angle);
        }

        private void UpdateCenterPosition(Vector2 position)
            => _map.style.translate = new Translate(-position.x, -position.y);

        private Vector2 WorldToMapPosition(Vector3 position)
        {
            Vector3 worldMin = _miniMap.worldBounds.min;
            Vector3 worldMax = _miniMap.worldBounds.max;

            Vector3 min = _miniMap.mapBounds.min;
            Vector3 max = _miniMap.mapBounds.max;

            float x = MathExtensions.Remap(position.x, worldMin.x, worldMax.x, min.x, max.x);

            // should be used 'max' value as 'min', because origin of the map coordinates
            // goes from top to bottom, and the world coordinates from bottom to top 
            float y = MathExtensions.Remap(position.z, worldMin.z, worldMax.z, max.y, min.y);

            return new Vector2(x, y);
        }

        private void Reset()
        {
            _hero = null;
            _miniMap = default;

            _map.style.backgroundImage = null;
            _map.style.translate = default;

            _mapWrapper.style.rotate = default;
        }

        private void Hide()
        {
            _isActive = false;
            _container.SetVisibility(false);
        }

        private void Show()
        {
            _isActive = true;
            _container.SetVisibility(true);
        }

        private static bool MapExists(ILocationDefinition definition)
            => definition.MapImage;
    }
}
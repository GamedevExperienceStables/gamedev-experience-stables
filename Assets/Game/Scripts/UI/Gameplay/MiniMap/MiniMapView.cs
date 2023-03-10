using Game.Level;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using VContainer.Unity;

namespace Game.UI
{
    public class MiniMapView : ILateTickable
    {
        private readonly MiniMapViewModel _viewModel;
        private VisualElement _playerRepresentation;
        private VisualElement _mapContainer;
        private VisualElement _mapImage;

        [Range(1, 15)]
        private float miniMultiplyer = 1f;

        private bool _isCreated;
        
        [Inject]
        public MiniMapView(MiniMapViewModel viewModel)
            => _viewModel = viewModel;

        public void Create(VisualElement root)
        {
            CreateMap(root);
            _isCreated = true;
        }

        private void CreateMap(VisualElement root)
        {
            _mapContainer = root.Q<VisualElement>("map");
            _mapImage = root.Q<VisualElement>("image");
            _playerRepresentation = root.Q<VisualElement>("player");
        }

        public void LateTick()
        {
            if (!_isCreated) 
                return;
            
            Vector3 heroPosition = _viewModel.HeroPosition;
            _playerRepresentation.style.translate = new Translate(-heroPosition.x * miniMultiplyer,
                -heroPosition.z * -miniMultiplyer, 0);
            _playerRepresentation.style.rotate = new Rotate(new Angle(_viewModel.HeroRotation.eulerAngles.y));

            //Calculate the width/height bounds for the map image
            var clampWidth = _mapImage.worldBound.width / 2 - _mapContainer.worldBound.width / 2;
            var clampHeight = _mapImage.worldBound.height / 2 - _mapContainer.worldBound.height / 2;

            //Clamp the bounds so that the map doesn't scroll past the playable area (i.e. the map image)
            var xPos = Mathf.Clamp(-heroPosition.x * -miniMultiplyer, -clampWidth, clampWidth);
            var yPos = Mathf.Clamp(-heroPosition.z * miniMultiplyer, -clampHeight, clampHeight);

            //Move the map image
            _mapImage.style.translate = new Translate(xPos, yPos, 0);
        }
    }
}
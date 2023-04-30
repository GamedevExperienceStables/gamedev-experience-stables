using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class ArtTemplateView
    {
        private VisualElement _artContainer;
        private VisualElement _image;
        
        public void Create(VisualElement root)
        {
            
            _artContainer = root.Q<VisualElement>("art-container");
            _image = root.Q<VisualElement>("art");
        }

        public void SetData(Sprite sprite) 
            => _image.style.backgroundImage = new StyleBackground(sprite);
    }
}
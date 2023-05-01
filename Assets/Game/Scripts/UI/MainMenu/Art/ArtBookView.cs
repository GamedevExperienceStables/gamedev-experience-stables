using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class ArtBookView
    {
        private readonly List<ArtTemplateView> _arts = new();
        private readonly ArtBookData _settings;
        
        private VisualElement _listContent;
        private VisualTreeAsset _template;
        
        private readonly ArtTemplateViewFactory _artFactory;
        
        public ArtBookView(ArtSettings settings, ArtTemplateViewFactory artFactory)
        {
            _settings = settings.ArtBookData;
            _artFactory = artFactory;
        }
        
        public void Create(VisualElement root)
        {
            _listContent = root.Q<VisualElement>("art-content");
            _template = _listContent.Q<TemplateContainer>("page-template").templateSource;
            
            _listContent.Clear();

            CreateArts();
        }
        
        private void CreateArts()
        {
            foreach (Sprite image in _settings.icons)
            {
                TemplateContainer element = _template.Instantiate();
                _listContent.Add(element);

                ArtTemplateView artView = _artFactory.Create(element, image);
                _arts.Add(artView);
            }
        }
    }
}
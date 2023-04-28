using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class ArtTemplateViewFactory
    {
        private readonly IObjectResolver _resolver;

        public ArtTemplateViewFactory(IObjectResolver resolver) 
            => _resolver = resolver;

        public ArtTemplateView Create(TemplateContainer element, Sprite sprite)
        {
            var view = _resolver.Resolve<ArtTemplateView>();
            
            view.Create(element);
            view.SetData(sprite);

            return view;
        }
    }
}
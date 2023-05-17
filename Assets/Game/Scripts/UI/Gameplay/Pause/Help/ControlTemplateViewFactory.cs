using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class ControlTemplateViewFactory
    {
        private readonly IObjectResolver _resolver;

        public ControlTemplateViewFactory(IObjectResolver resolver) 
            => _resolver = resolver;

        public ControlTemplateView Create(TemplateContainer element, HelpContentData.Control control)
        {
            var view = _resolver.Resolve<ControlTemplateView>();
            
            view.Create(element);
            view.SetData(control);

            return view;
        }
    }
}
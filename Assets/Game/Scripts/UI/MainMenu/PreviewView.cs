using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class PreviewView
    {
        private readonly VisualElement _content;
        
        private readonly VisualElement _image;
        private readonly Label _caption;

        public PreviewView(VisualElement content)
        {
            _content = content;
            
            _image = _content.Q<VisualElement>(LayoutNames.StartMenu.PREVIEW_IMAGE);
            _caption = _content.Q<Label>(LayoutNames.StartMenu.PREVIEW_CAPTION);
        }

        public void Show()
        {
            _content.SetVisibility(true);
        }

        public void Show(Sprite preview, string caption)
        {
            _content.SetVisibility(true);
            _image.style.backgroundImage = new StyleBackground(preview);
            _caption.text = caption;
        }

        public void Hide()
        {
            _content.SetVisibility(false);
        }
    }
}
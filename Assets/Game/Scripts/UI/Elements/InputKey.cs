using Game.Input;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class InputKey : VisualElement
    {
        private const string CONTAINER_CLASS_NAME = "input-key";
        private const string AS_TEXT_CLASS_NAME = "input-key--text";
        private const string AS_ICON_CLASS_NAME = "input-key--icon";

        private const string TEXT_CLASS_NAME = "input-key__text";
        private const string LABEL_CLASS_NAME = "input-key__label";
        private const string ICON_CLASS_NAME = "input-key__icon";

        private readonly Label _label;
        private readonly Image _icon;

        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        public InputKey()
        {
            AddToClassList(CONTAINER_CLASS_NAME);
            usageHints = UsageHints.DynamicColor;
            
            var textContainer = new VisualElement();
            textContainer.AddToClassList(TEXT_CLASS_NAME);
            Add(textContainer);
            
            _label = new Label
            {
                text = "#"
            };
            _label.AddToClassList(LABEL_CLASS_NAME);
            textContainer.Add(_label);

            _icon = new Image();
            _icon.AddToClassList(ICON_CLASS_NAME);
            Add(_icon);

            ShowText();
        }
        
        public void Bind(InputKeyBinding binding)
        {
            _label.text = binding.Key;

            if (binding.Icon)
            {
                _icon.sprite = binding.Icon;
                
                ShowIcon();
            }
        }

        public void ShowIcon()
        {
            RemoveFromClassList(AS_TEXT_CLASS_NAME);
            AddToClassList(AS_ICON_CLASS_NAME);
        }

        private void ShowText()
        {
            RemoveFromClassList(AS_ICON_CLASS_NAME);
            AddToClassList(AS_TEXT_CLASS_NAME);
        }

        public new class UxmlFactory : UxmlFactory<InputKey, UxmlTraits>
        {
        }
        
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _text = new() { name = "text", defaultValue = "#" };

            public override void Init(VisualElement element, IUxmlAttributes attr, CreationContext cc)
            {
                base.Init(element, attr, cc);

                if (element is not InputKey inputKey)
                    return;

                inputKey.Text = _text.GetValueFromBag(attr, cc);
            }
        }
    }
}
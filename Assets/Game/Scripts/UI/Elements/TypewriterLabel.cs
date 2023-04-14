using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class TypewriterLabel : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<TypewriterLabel, UxmlTraits>
        {
        }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            readonly UxmlStringAttributeDescription text = new() { name = "text", defaultValue = "" };

            readonly UxmlIntAttributeDescription maxVisibleCharacter =
                new() { name = "maxVisibleCharacters", defaultValue = 0 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement element, IUxmlAttributes attr, CreationContext cc)
            {
                base.Init(element, attr, cc);

                if (element is not TypewriterLabel typewriterLabel)
                {
                    return;
                }

                typewriterLabel.Text = text.GetValueFromBag(attr, cc);
                typewriterLabel.MaxVisibleCharacters = maxVisibleCharacter.GetValueFromBag(attr, cc);
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                SetChildText();
            }
        }

        public int MaxVisibleCharacters
        {
            get => _maxVisibleCharacters;
            set
            {
                _maxVisibleCharacters = value;
                SetChildText();
            }
        }

        public bool IsTextFullyVisible => _maxVisibleCharacters >= _text?.Length;

        private readonly Label _childLabel;
        private string _text;
        private int _maxVisibleCharacters;

        public TypewriterLabel()
        {
            _childLabel = new Label
            {
                enableRichText = true, style =
                {
                    fontSize = style.fontSize, unityFont = style.unityFont,
                    unityFontDefinition = style.unityFontDefinition, color = style.color,
                    whiteSpace = new StyleEnum<WhiteSpace>(WhiteSpace.Normal),
                    unityFontStyleAndWeight = style.unityFontStyleAndWeight
                }
            };
            Add(_childLabel);
        }

        private void SetChildText()
        {
            if (_text?.Length > 0)
            {
                int len = math.min(_maxVisibleCharacters, _text.Length);
                _childLabel.text = $"{_text[..len]}<alpha=#00>{_text[len..]}";
            }
            else
            {
                _childLabel.text = string.Empty;
            }
        }
    }
}
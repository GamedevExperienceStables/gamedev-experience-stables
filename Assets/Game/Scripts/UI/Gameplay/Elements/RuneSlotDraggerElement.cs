using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI.Elements
{
    public class RuneSlotDraggerElement : VisualElement
    {
        private readonly Image _icon;

        public RuneSlotDraggerElement()
        {
            _icon = new Image
            {
                name = "Icon"
            };
            
            Add(_icon);
        }

        public void StartDrag(Sprite icon)
        {
            _icon.sprite = icon;
            
            AddToClassList(LayoutNames.Inventory.RUNE_DRAGGER_DRAG_CLASS_NAME);
        }

        public void StopDrag()
        {
            _icon.image = null;
            
            RemoveFromClassList(LayoutNames.Inventory.RUNE_DRAGGER_DRAG_CLASS_NAME);
        }

        public new class UxmlFactory : UxmlFactory<RuneSlotDraggerElement, UxmlTraits>
        {
        }
    }
}
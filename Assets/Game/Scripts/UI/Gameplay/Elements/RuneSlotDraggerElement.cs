using Game.Inventory;
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

        public RuneDefinition Rune { get; private set; }

        public void StartDrag(RuneDefinition definition)
        {
            Rune = definition;

            _icon.image = definition.Icon.texture;
            
            AddToClassList(LayoutNames.Inventory.RUNE_DRAGGER_DRAG_CLASS_NAME);
        }

        public void StopDrag()
        {
            Rune = null;
            _icon.image = null;
            
            RemoveFromClassList(LayoutNames.Inventory.RUNE_DRAGGER_DRAG_CLASS_NAME);
        }

        public new class UxmlFactory : UxmlFactory<RuneSlotDraggerElement, UxmlTraits>
        {
        }
    }
}
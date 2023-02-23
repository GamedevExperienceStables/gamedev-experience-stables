using System;
using Game.Inventory;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class RuneSlotInventoryView
    {
        private readonly Image _icon;
        private readonly RuneDefinition _runeDefinition;
        private readonly VisualElement _element;
        
        private event Action<RuneSlotDragEvent> PointerDownCallback;

        public RuneSlotInventoryView(VisualElement element, RuneDefinition runeDefinition)
        {
            _element = element;
            _runeDefinition = runeDefinition;

            _icon = element.Q<Image>(LayoutNames.Inventory.RUNE_SLOT_ICON);
            if (runeDefinition.Icon)
                _icon.image = runeDefinition.Icon.texture;
            
            Deactivate();
            
            element.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }
        
        public void Activate()
        {
            _element.SetEnabled(true);
        }

        public void Deactivate()
        {
            _element.SetEnabled(false);
        }

        public void UseSlot()
        {
            _element.SetEnabled(false);
        }

        public void ReleaseSlot()
        {
            _element.SetEnabled(true);
        }

        public bool Contains(RuneDefinition definition)
            => ReferenceEquals(_runeDefinition, definition);

        public void SubscribeStartDrag(Action<RuneSlotDragEvent> callback)
            => PointerDownCallback += callback;

        public void UnSubscribeStartDrag(Action<RuneSlotDragEvent> callback)
            => PointerDownCallback -= callback;

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (_runeDefinition == null)
                return;

            if (_runeDefinition.Type == RuneType.Passive)
                return;

            if (!evt.IsLeftButton())
                return;

            var dragEvent = new RuneSlotDragEvent
            {
                pointerId = evt.pointerId,
                pointerPosition = evt.position,
                elementPosition = GetWorldPosition(),
                definition = _runeDefinition
            };
            PointerDownCallback?.Invoke(dragEvent);
        }

        private Vector2 GetWorldPosition() 
            => _element.worldBound.position - _element.parent.worldBound.position;
    }
}
using System;
using Game.Inventory;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class RuneSlotHudView
    {
        private readonly Image _icon;
        private bool _interactable;
        private RuneDefinition _runeDefinition;

        private event Action<RuneSlotRemoveEvent> RuneRemovingRequest;

        public RuneSlotHudView(VisualElement element, RuneSlotId id)
        {
            Id = id;
            Element = element;

            _icon = element.Q<Image>(LayoutNames.Hud.RUNE_SLOT_ICON);

            element.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        public RuneSlotId Id { get; }

        public VisualElement Element { get; }
        
        public void SubscribeRemovingRequest(Action<RuneSlotRemoveEvent> callback) 
            => RuneRemovingRequest += callback;

        public void UnSubscribeRemovingRequest(Action<RuneSlotRemoveEvent> callback) 
            => RuneRemovingRequest -= callback;

        public void Clear()
        {
            _runeDefinition = null;
            _icon.image = null;
        }

        public void Set(RuneDefinition rune)
        {
            _runeDefinition = rune;
            
            if (rune.Icon)
                _icon.image = rune.Icon.texture;
        }

        public void Activate()
            => Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);

        public void Deactivate()
            => Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);


        private void OnPointerDown(PointerDownEvent evt)
        {
            if (!_interactable)
                return;

            if (_runeDefinition == null)
                return;

            if (evt.IsLeftButton())
                Debug.Log("Clicked");
            
            if (evt.IsRightButton())
                RemoveRuneFromSlot(evt);
        }

        private void RemoveRuneFromSlot(IPointerEvent evt)
        {
            var removeEvent = new RuneSlotRemoveEvent
            {
                id = Id,
                pointerId = evt.pointerId,
                definition = _runeDefinition,
            };
            
            RuneRemovingRequest?.Invoke(removeEvent);
        }

        public void EnableInteraction()
        {
            _interactable = true;
            Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_INTERACTABLE_CLASS_NAME);
        }

        public void DisableInteraction()
        {
            _interactable = false;
            Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_INTERACTABLE_CLASS_NAME);
        }
    }
}
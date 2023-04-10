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
        private readonly VisualElement _background;
        private bool _interactable;
        private RuneDefinition _runeDefinition;

        private event Action<RuneSlotRemoveEvent> RuneRemovingRequest;

        public RuneSlotHudView(VisualElement element, RuneSlotId id)
        {
            Id = id;
            Element = element;

            _icon = element.Q<Image>(LayoutNames.Hud.RUNE_SLOT_ICON);
            _background = element.Q<VisualElement>(LayoutNames.Hud.RUNE_SLOT_BACKGROUND);

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
            _icon.sprite = null;
            _background.visible = true;
        }

        public void Set(RuneDefinition rune)
        {
            _runeDefinition = rune;

            if (!rune.Icon)
                return;
            
            _icon.sprite = rune.Icon;
            _background.visible = false;
        }

        public void Activate()
            => _icon.sprite = _runeDefinition.IconActive;

        public void Deactivate()
            => _icon.sprite = _runeDefinition.Icon;


        private void OnPointerDown(PointerDownEvent evt)
        {
            if (!_interactable)
                return;

            if (_runeDefinition == null)
                return;

            if (evt.IsLeftButton())
            {
#if UNITY_EDITOR
                Debug.Log("Clicked");
#endif
            }

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
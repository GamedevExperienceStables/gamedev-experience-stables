﻿using System;
using Game.Input;
using Game.Inventory;
using Game.Utils;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class RuneSlotHudView
    {
        private readonly Image _icon;
        private bool _interactable;
        private RuneDefinition _runeDefinition;

        private event Action<RuneSlotRemoveEvent> RuneRemovingRequest;
        private event Action<RuneSlotDragEvent> PointerDownCallback;

        public RuneSlotHudView(VisualElement element, RuneSlotId id, InputKeyBinding inputSelect, InputKeyBinding inputActive)
        {
            Id = id;
            Element = element;

            _icon = element.Q<Image>(LayoutNames.Hud.RUNE_SLOT_ICON);
            element.Q<VisualElement>(LayoutNames.Hud.RUNE_SLOT_BACKGROUND);

            var inputKeySelect = element.Q<InputKey>(LayoutNames.Hud.RUNE_SLOT_INPUT_SELECT);
            inputKeySelect.Bind(inputSelect);

            var inputKeyActive = element.Q<InputKey>(LayoutNames.Hud.RUNE_SLOT_INPUT_ACTIVE);
            inputKeyActive.Bind(inputActive);

            Clear();

            _icon.RegisterCallback<PointerDownEvent>(OnPointerDown);
        }

        public RuneSlotId Id { get; }

        public VisualElement Element { get; }

        public void Destroy() 
            => _icon.UnregisterCallback<PointerDownEvent>(OnPointerDown);

        public void SubscribeStartDrag(Action<RuneSlotDragEvent> callback)
            => PointerDownCallback += callback;

        public void UnSubscribeStartDrag(Action<RuneSlotDragEvent> callback)
            => PointerDownCallback -= callback;

        public void SubscribeRemovingRequest(Action<RuneSlotRemoveEvent> callback)
            => RuneRemovingRequest += callback;

        public void UnSubscribeRemovingRequest(Action<RuneSlotRemoveEvent> callback)
            => RuneRemovingRequest -= callback;

        public void Clear()
        {
            _runeDefinition = null;

            _icon.sprite = null;

            Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_EMPTY_CLASS_NAME);
        }

        public void Set(RuneDefinition rune)
        {
            _runeDefinition = rune;

            _icon.sprite = rune.Icon ? rune.Icon : null;

            Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_EMPTY_CLASS_NAME);
        }

        public void Activate()
        {
            _icon.sprite = _runeDefinition.IconActive;

            Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);
        }

        public void Deactivate()
        {
            _icon.sprite = _runeDefinition.Icon;

            Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);
        }


        private void OnPointerDown(PointerDownEvent evt)
        {
            if (!_interactable)
                return;

            if (!_runeDefinition)
                return;

            if (evt.IsLeftButton()) 
                StartDrag(evt);

            if (evt.IsRightButton())
                RemoveRuneFromSlot(evt);
        }

        private void StartDrag(IPointerEvent evt)
        {
            var dragEvent = new RuneSlotDragEvent
            {
                source = this,
                pointerId = evt.pointerId,
                pointerPosition = evt.position,
                elementPosition = Element.worldBound.position,
                definition = _runeDefinition,
                
                onStopped = OnDragStopped
            };
            
            Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_DRAG_CLASS_NAME);

            PointerDownCallback?.Invoke(dragEvent);
        }

        private void OnDragStopped() 
            => Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_DRAG_CLASS_NAME);


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

        public void SetEnabled(bool isEnabled)
        {
            if (isEnabled)
                Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_ENABLED_CLASS_NAME);
            else
                Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_ENABLED_CLASS_NAME);
        }
    }
}
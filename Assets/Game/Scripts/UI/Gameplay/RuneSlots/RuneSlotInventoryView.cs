﻿using System;
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
        private event Action<RuneSlotHoverEvent> PointerHoverCallback;

        public RuneSlotInventoryView(VisualElement element, RuneDefinition runeDefinition)
        {
            _element = element;
            _runeDefinition = runeDefinition;

            _icon = element.Q<Image>(LayoutNames.Inventory.RUNE_SLOT_ICON);
            if (runeDefinition.Icon)
                _icon.sprite = runeDefinition.Icon;
            
            Deactivate();
            
            element.RegisterCallback<PointerDownEvent>(OnPointerDown);
            element.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            element.RegisterCallback<PointerOutEvent>(OnPointerOut);
        }

        public void Activate()
        {
            _element.SetVisibility(true);
        }

        public void Deactivate()
        {
            _element.SetVisibility(false);
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
        
        public void SubscribeHover(Action<RuneSlotHoverEvent> callback)
            => PointerHoverCallback += callback;

        public void UnsubscribeHover(Action<RuneSlotHoverEvent> callback)
            => PointerHoverCallback -= callback;

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
                elementPosition = _element.worldBound.position,
                definition = _runeDefinition
            };
            PointerDownCallback?.Invoke(dragEvent);
        }

        private void OnPointerEnter(PointerEnterEvent evt)
        {
            OnHover(evt, true);
        }

        private void OnPointerOut(PointerOutEvent evt)
        {
            OnHover(evt, false);
        }

        private void OnHover(IPointerEvent evt, bool state)
        {
            if (_runeDefinition == null)
                return;

            var hoverEvent = new RuneSlotHoverEvent()
            {
                pointerId = evt.pointerId,
                definition = _runeDefinition,
                state = state
            };
            PointerHoverCallback?.Invoke(hoverEvent);
        }

        private Vector2 GetWorldPosition() 
            => _element.worldBound.position - _element.parent.worldBound.position;
    }
}
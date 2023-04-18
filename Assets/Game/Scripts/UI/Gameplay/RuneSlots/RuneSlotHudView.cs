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

        private readonly Label _inputLabel;

        private readonly string _inputSelect;
        private readonly string _inputActive;

        private event Action<RuneSlotRemoveEvent> RuneRemovingRequest;

        public RuneSlotHudView(VisualElement element, RuneSlotId id, string inputSelect, string inputActive)
        {
            Id = id;
            Element = element;

            _icon = element.Q<Image>(LayoutNames.Hud.RUNE_SLOT_ICON);
            element.Q<VisualElement>(LayoutNames.Hud.RUNE_SLOT_BACKGROUND);

            _inputSelect = inputSelect;
            _inputActive = inputActive;

            _inputLabel = element.Q<Label>(LayoutNames.Hud.RUNE_SLOT_INPUT_LABEL);
            _inputLabel.text = inputSelect;

            Clear();

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
            _inputLabel.text = _inputActive;

            Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);
        }

        public void Deactivate()
        {
            _icon.sprite = _runeDefinition.Icon;
            _inputLabel.text = _inputSelect;

            Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_ACTIVE_CLASS_NAME);
        }


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

        public void SetEnabled(bool isEnabled)
        {
            if (isEnabled)
                Element.AddToClassList(LayoutNames.Hud.RUNE_SLOT_ENABLED_CLASS_NAME);
            else
                Element.RemoveFromClassList(LayoutNames.Hud.RUNE_SLOT_ENABLED_CLASS_NAME);
        }
    }
}
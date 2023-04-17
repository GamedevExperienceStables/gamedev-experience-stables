using System.Collections.Generic;
using Game.Input;
using Game.Inventory;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class HudRuneSlotsView
    {
        private readonly Dictionary<RuneSlotId, RuneSlotHudView> _hudSlots = new();
        private readonly HudRuneSlotsViewModel _viewModel;
        private readonly InputBindings _input;

        [Inject]
        public HudRuneSlotsView(HudRuneSlotsViewModel viewModel, InputBindings input)
        {
            _viewModel = viewModel;
            _input = input;
        }

        public IReadOnlyDictionary<RuneSlotId, RuneSlotHudView> Slots => _hudSlots;

        public void Create(VisualElement root)
        {
            CreateSlots(root);
            InitSlots(_viewModel.Slots);

            _viewModel.SubscribeActiveRuneSlotChanged(OnActiveRuneSlotChanged);
            _viewModel.SubscribeRuneSlotChanged(OnRuneSlotsChanged);
        }

        public void Destroy()
        {
            _viewModel.UnSubscribeActiveRuneSlotChanged(OnActiveRuneSlotChanged);
            _viewModel.UnSubscribeRuneSlotsChanges(OnRuneSlotsChanged);
        }

        public void LateTick()
            => UpdateSlotsState();

        private void CreateSlots(VisualElement root)
        {
            var slots = root
                .Query<VisualElement>(className: LayoutNames.Hud.RUNE_SLOT_CLASS_NAME)
                .ToList();

            string inputActive = _input.GetBindingDisplayString(InputGameplayActions.Fire);
            for (int i = 0; i < slots.Count; i++)
            {
                int slotId = i + 1;
                VisualElement slotElement = slots[i];

                string inputSelect = GetInput(slotId);

                var slotView = new RuneSlotHudView(slotElement, slotId, inputSelect, inputActive);
                _hudSlots.Add(slotId, slotView);
            }
        }

        private string GetInput(int slotId) =>
            slotId switch
            {
                1 => _input.GetBindingDisplayString(InputGameplayActions.Slot1),
                2 => _input.GetBindingDisplayString(InputGameplayActions.Slot2),
                3 => _input.GetBindingDisplayString(InputGameplayActions.Slot3),
                4 => _input.GetBindingDisplayString(InputGameplayActions.Slot4),
                _ => string.Empty
            };

        private void InitSlots(IReadOnlyDictionary<RuneSlotId, RuneSlot> slots)
        {
            foreach ((RuneSlotId key, RuneSlot runeSlot) in slots)
            {
                if (_hudSlots.TryGetValue(key, out RuneSlotHudView slotView) && !runeSlot.IsEmpty)
                    slotView.Set(runeSlot.Rune);
            }
        }

        private void UpdateSlotsState()
        {
            foreach (RuneSlot slot in _viewModel.Slots.Values)
            {
                if (!slot.IsEmpty)
                    UpdateSlotState(slot);
            }
        }

        private void UpdateSlotState(RuneSlot slot)
        {
            bool isEnabled = _viewModel.CanActivate(slot.Rune);
            _hudSlots[slot.Id].SetEnabled(isEnabled);
        }


        private void OnRuneSlotsChanged(RuneSlotChangedEvent changed)
        {
            if (_hudSlots.TryGetValue(changed.id, out RuneSlotHudView slotView))
                UpdateSlotView(slotView, changed.definition);
        }

        private void OnActiveRuneSlotChanged(RuneActiveSlotChangedEvent changed)
        {
            if (_hudSlots.TryGetValue(changed.oldId, out RuneSlotHudView oldSlot))
                oldSlot.Deactivate();

            if (_hudSlots.TryGetValue(changed.newId, out RuneSlotHudView newSlot))
                newSlot.Activate();
        }

        private static void UpdateSlotView(RuneSlotHudView slotView, RuneDefinition runeDefinition)
        {
            if (runeDefinition)
                slotView.Set(runeDefinition);
            else
                slotView.Clear();
        }
    }
}
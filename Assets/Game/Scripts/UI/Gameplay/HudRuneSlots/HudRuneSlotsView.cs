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
        private readonly HudRunesFx _fx;

        [Inject]
        public HudRuneSlotsView(HudRuneSlotsViewModel viewModel, InputBindings input, HudRunesFx fx)
        {
            _viewModel = viewModel;
            _input = input;
            _fx = fx;
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

            foreach (RuneSlotHudView slot in _hudSlots.Values)
                slot.Destroy();

            _fx.Destroy();
        }

        public void LateTick()
            => UpdateSlotsState();

        private void CreateSlots(VisualElement root)
        {
            var slots = root
                .Query<VisualElement>(className: LayoutNames.Hud.RUNE_SLOT_CLASS_NAME)
                .ToList();

            InputKeyBinding inputActive = _input.GetBindingDisplay(InputGameplayActions.Fire);
            for (int i = 0; i < slots.Count; i++)
            {
                int slotId = i + 1;
                VisualElement slotElement = slots[i];

                InputKeyBinding inputSelect = GetInput(slotId);

                var slotView = new RuneSlotHudView(slotElement, slotId, inputSelect, inputActive);

                _hudSlots.Add(slotId, slotView);
            }
        }

        private InputKeyBinding GetInput(int slotId) =>
            slotId switch
            {
                1 => _input.GetBindingDisplay(InputGameplayActions.Slot1),
                2 => _input.GetBindingDisplay(InputGameplayActions.Slot2),
                3 => _input.GetBindingDisplay(InputGameplayActions.Slot3),
                4 => _input.GetBindingDisplay(InputGameplayActions.Slot4),
                _ => default
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

            if (changed.newId.IsValid())
                _fx.ActivateFeedback();
            else if (changed.oldId.IsValid())
                _fx.DeactivateFeedback();
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
using System.Collections.Generic;
using Game.Inventory;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class HudRuneSlotsView
    {
        private readonly List<RuneSlotHudView> _hudSlots = new();
        private readonly HudRuneSlotsViewModel _viewModel;

        [Inject]
        public HudRuneSlotsView(HudRuneSlotsViewModel viewModel)
            => _viewModel = viewModel;

        public IReadOnlyList<RuneSlotHudView> Slots => _hudSlots;

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

        private void CreateSlots(VisualElement root)
        {
            var slots = root
                .Query<VisualElement>(className: LayoutNames.Hud.RUNE_SLOT_CLASS_NAME)
                .ToList();

            for (int i = 0; i < slots.Count; i++)
            {
                int slotId = i + 1;
                VisualElement slotElement = slots[i];

                var slotView = new RuneSlotHudView(slotElement, slotId);
                _hudSlots.Add(slotView);
            }
        }

        private void InitSlots(IReadOnlyDictionary<RuneSlotId, RuneSlot> slots)
        {
            foreach ((RuneSlotId key, RuneSlot runeSlot) in slots)
            {
                foreach (RuneSlotHudView hudRuneSlotView in _hudSlots)
                {
                    if (hudRuneSlotView.Id != key || runeSlot.IsEmpty)
                        continue;

                    hudRuneSlotView.Set(runeSlot.Rune);
                    break;
                }
            }
        }

        private void OnRuneSlotsChanged(RuneSlotChangedEvent changed)
        {
            foreach (RuneSlotHudView slotView in _hudSlots)
            {
                if (slotView.Id != changed.id)
                    continue;

                UpdateSlotView(slotView, changed.definition);
                break;
            }
        }

        private void OnActiveRuneSlotChanged(RuneActiveSlotChangedEvent changed)
        {
            foreach (RuneSlotHudView slotView in _hudSlots)
            {
                if (slotView.Id == changed.oldId)
                    slotView.Deactivate();

                if (slotView.Id == changed.newId)
                    slotView.Activate();
            }
        }

        private static void UpdateSlotView(RuneSlotHudView slotView, RuneDefinition runeDefinition)
        {
            if (runeDefinition == null)
                slotView.Clear();
            else
                slotView.Set(runeDefinition);
        }
    }
}
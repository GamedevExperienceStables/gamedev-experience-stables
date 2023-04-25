using System;
using System.Collections.Generic;
using Game.Actors;
using VContainer;

namespace Game.Inventory
{
    public class InventoryController : IInventoryItems, IInventorySlots, IInventoryRunes
    {
        private readonly RuneSlots _slots;
        private readonly Materials _materials;
        private readonly Runes _runes;

        [Inject]
        public InventoryController(InventoryData data)
        {
            _materials = data.Materials;
            _runes = data.Runes;
            _slots = data.Slots;
        }

        public IReadOnlyMaterials Materials => _materials;
        public IReadOnlyRunes Runes => _runes;
        public IReadOnlyRuneSlots Slots => _slots;

        IReadOnlyDictionary<RuneSlotId, RuneSlot> IInventorySlots.Items => _slots.Items;

        public event Action<RuneActiveSlotChangedEvent> ActiveSlotChanged;
        public event Action<RuneSlotChangedEvent> SlotChanged;

        public bool HasActive => _slots.HasActive;
        public RuneSlot ActiveSlot => _slots.GetActive();

        public void Reset()
        {
            _materials.Reset();
            _runes.Reset();
            _slots.Reset();
        }

        public void Init(InventoryInitialData data)
        {
            _materials.Init(data.container, data.bag);
            _runes.Init(data.runes);
            _slots.Init(data.slots);
        }

        public bool CanTransferToContainer(MaterialDefinition levelMaterial)
            => _materials.CanTransferToContainer(levelMaterial);

        public void TransferToContainer(MaterialDefinition levelMaterial)
            => _materials.TransferToContainer(levelMaterial);

        public bool IsContainerFull(MaterialDefinition levelMaterial)
            => _materials.Container.IsFull(levelMaterial);

        public bool IsBagEmpty(MaterialDefinition levelMaterial)
            => _materials.Bag.IsEmpty(levelMaterial);

        public bool CanAddToBag(ItemDefinition definition, IActorController owner)
            => definition switch
            {
                IItemExecutableDefinition executableItem => executableItem.CanExecute(
                    new ItemExecutionContext(owner, this)),
                MaterialDefinition material => _materials.CanAddToBag(material),
                _ => false
            };

        public bool TryAddToBag(ItemDefinition item, IActorController owner)
        {
            if (!CanAddToBag(item, owner))
                return false;

            AddToBag(item, owner);
            return true;
        }

        public void ClearBag()
            => _materials.ClearBag();

        public void AddToBag(ItemDefinition item, IActorController owner)
        {
            if (item is IItemExecutableDefinition executableItem)
                executableItem.Execute(new ItemExecutionContext(owner, this));

            switch (item)
            {
                case MaterialDefinition material:
                    _materials.AddToBag(material);
                    break;

                case RuneDefinition rune:
                    _runes.Add(rune);
                    break;
            }
        }

        public void SetActiveSlot(RuneSlotId slotId)
        {
            if (_slots.IsEmpty(slotId))
                return;

            RuneActiveSlotChangedEvent changedEvent;
            if (_slots.IsActive(slotId))
            {
                _slots.ClearActive();

                changedEvent = new RuneActiveSlotChangedEvent
                {
                    oldId = slotId,
                };
            }
            else
            {
                RuneSlotId oldId = default;
                if (_slots.HasActive)
                    oldId = _slots.GetActive().Id;

                _slots.SetActive(slotId);

                changedEvent = new RuneActiveSlotChangedEvent
                {
                    newId = slotId,
                    oldId = oldId,
                };
            }

            ActiveSlotChanged?.Invoke(changedEvent);
        }

        public void SwapSlots(RuneSlotId slotId1, RuneSlotId slotId2)
        {
            RuneDefinition rune1 = _slots.Get(slotId1);
            RuneDefinition rune2 = _slots.Get(slotId2);

            if (rune1)
                SetSlot(slotId2, rune1);
            else
                ClearSlot(slotId2);

            if (rune2)
                SetSlot(slotId1, rune2);
            else
                ClearSlot(slotId1);
        }

        public void SetRuneToSlot(RuneSlotId slotId, RuneDefinition targetRune)
        {
            bool containsInAnotherSlot = _slots.Find(targetRune, out RuneSlotId foundSlotId);
            if (containsInAnotherSlot)
                ClearSlot(foundSlotId);

            bool targetSlotIsEmpty = _slots.IsEmpty(slotId);
            if (!targetSlotIsEmpty)
                _slots.Clear(slotId);

            SetSlot(slotId, targetRune);
        }

        private void SetSlot(RuneSlotId slotId, RuneDefinition targetRune)
        {
            _slots.Set(slotId, targetRune);
            SlotChanged?.Invoke(new RuneSlotChangedEvent
            {
                id = slotId,
                definition = targetRune,
            });

            if (_slots.IsActive(slotId))
            {
                ActiveSlotChanged?.Invoke(new RuneActiveSlotChangedEvent
                {
                    oldId = slotId,
                    newId = slotId,
                });
            }
        }

        public void ClearSlot(RuneSlotId slotId)
        {
            if (_slots.IsEmpty(slotId))
                return;

            if (_slots.IsActive(slotId))
                ClearActive(slotId);

            _slots.Clear(slotId);
            SlotChanged?.Invoke(new RuneSlotChangedEvent
            {
                id = slotId,
                definition = null
            });
        }

        private void ClearActive(RuneSlotId slotId)
        {
            _slots.ClearActive();
            ActiveSlotChanged?.Invoke(new RuneActiveSlotChangedEvent
            {
                oldId = slotId
            });
        }

        public IReadOnlyList<RuneDefinition> Items => _runes.Items;

        public void Subscribe(Action<RuneDefinition> callback)
            => _runes.Subscribe(callback);

        public void UnSubscribe(Action<RuneDefinition> callback)
            => _runes.UnSubscribe(callback);
    }
}
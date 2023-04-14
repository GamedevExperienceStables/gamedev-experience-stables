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

        public void SetSlot(RuneSlotId slotId, RuneDefinition targetRune)
        {
            if (_slots.Find(targetRune, out RuneSlotId foundSlotId))
            {
                _slots.Clear(foundSlotId);
                SlotChanged?.Invoke(new RuneSlotChangedEvent
                {
                    id = foundSlotId
                });
            }

            if (!_slots.IsEmpty(slotId))
                _slots.Clear(slotId);

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
            {
                _slots.ClearActive();
                ActiveSlotChanged?.Invoke(new RuneActiveSlotChangedEvent
                {
                    oldId = slotId
                });
            }

            _slots.Clear(slotId);
            SlotChanged?.Invoke(new RuneSlotChangedEvent
            {
                id = slotId
            });
        }

        public IReadOnlyList<RuneDefinition> Items => _runes.Items;

        public void Subscribe(Action<RuneDefinition> callback)
            => _runes.Subscribe(callback);

        public void UnSubscribe(Action<RuneDefinition> callback)
            => _runes.UnSubscribe(callback);
    }
}
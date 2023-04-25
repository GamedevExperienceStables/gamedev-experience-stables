﻿using System;
using System.Collections.Generic;
using Game.GameFlow;
using Game.Inventory;
using VContainer;

namespace Game.UI
{
    public class InventoryViewModel
    {
        private readonly GameplayInventory _inventorySwitcher;
        private readonly RuneDataTable _dataTable;
        private readonly IInventoryRunes _runes;
        private readonly IInventorySlots _slots;

        [Inject]
        public InventoryViewModel(
            GameplayInventory inventorySwitcher, 
            IInventoryRunes runes,
            IInventorySlots slots,
            RuneDataTable dataTable)
        {
            _inventorySwitcher = inventorySwitcher;
            _runes = runes;
            _slots = slots;
            _dataTable = dataTable;
        }

        public IReadOnlyList<RuneDefinition> AllRunes => _dataTable.Items;
        public IReadOnlyList<RuneDefinition> ObtainedRunes => _runes.Items;

        public void CloseInventory()
            => _inventorySwitcher.Disable();

        public void SubscribeRuneAdded(Action<RuneDefinition> callback)
            => _runes.Subscribe(callback);

        public void UnSubscribeRuneAdded(Action<RuneDefinition> callback)
            => _runes.UnSubscribe(callback);

        public RuneDefinition GetRuneFromHudSlot(RuneSlotId targetId) 
            => _slots.GetRuneFromSlot(targetId);

        public void SetRuneToHudSlot(RuneSlotId slotId, RuneDefinition targetRune)
            => _slots.SetRuneToSlot(slotId, targetRune);

        public void RemoveRuneFromHudSlot(RuneSlotId slotId)
            => _slots.ClearSlot(slotId);
    }
}
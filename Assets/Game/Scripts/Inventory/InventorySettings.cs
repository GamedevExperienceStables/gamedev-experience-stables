using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Inventory
{
    [Serializable]
    public class InventorySettings
    {
        [SerializeField, Min(0)]
        private int bagMaxStack = 10;

        [SerializeField]
        private List<RuneDefinition> initialRunes;

        public int BagMaxStack => bagMaxStack;

        public int InventorySlots => 4;

        public List<RuneDefinition> InitialRunes => initialRunes;
    }
}
using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public class HudSettings
    {
        [SerializeField]
        private LocalizedString inventoryPrompt;

        public LocalizedString InventoryPrompt => inventoryPrompt;
    }
}
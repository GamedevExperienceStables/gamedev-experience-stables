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

        [SerializeField]
        private LocalizedString activeQuest;

        public LocalizedString InventoryPrompt => inventoryPrompt;

        public LocalizedString ActiveQuest => activeQuest;
    }
}
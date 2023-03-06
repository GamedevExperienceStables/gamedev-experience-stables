﻿using Game.Inventory;
using Game.Level;
using Game.Persistence;
using UnityEngine;
using AudioSettings = Game.Audio.AudioSettings;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        private LevelsSettings levelsSettings;

        [SerializeField]
        private CameraSettings cameraSettings;

        [SerializeField]
        private UiSettings uiSettings;

        [SerializeField]
        private LootSettings lootSettings;
        
        [SerializeField]
        private InventorySettings inventorySettings;

        [SerializeField]
        private AudioSettings audioSettings;

        [SerializeField]
        private MagnetSystem.Settings magnetSettings;

        [SerializeField]
        private PersistenceService.Settings saveSettings;

        public CameraSettings CameraSettings => cameraSettings;
        public UiSettings UiSettings => uiSettings;
        public LootSettings LootSettings => lootSettings;
        public LevelsSettings LevelsSettings => levelsSettings;
        public PersistenceService.Settings SaveSettings => saveSettings;
        public MagnetSystem.Settings MagnetSettings => magnetSettings;
        public InventorySettings InventorySettings => inventorySettings;

        public AudioSettings AudioSettings => audioSettings;
    }
}
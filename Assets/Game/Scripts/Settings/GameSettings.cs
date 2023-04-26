using Game.CursorManagement;
using Game.Input;
using Game.Inventory;
using Game.Level;
using Game.Persistence;
using UnityEngine;
using UnityEngine.Serialization;
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

        [FormerlySerializedAs("uiSettings")]
        [SerializeField]
        private FaderSettings faderSettings;

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

        [SerializeField]
        private CursorService.Settings cursorSettings;
        
        [SerializeField]
        private InputBindingsSettings inputBindings;


        public CameraSettings CameraSettings => cameraSettings;
        public FaderSettings FaderSettings => faderSettings;
        public LootSettings LootSettings => lootSettings;
        public LevelsSettings LevelsSettings => levelsSettings;
        public PersistenceService.Settings SaveSettings => saveSettings;
        public MagnetSystem.Settings MagnetSettings => magnetSettings;
        public InventorySettings InventorySettings => inventorySettings;
        public AudioSettings AudioSettings => audioSettings;
        public CursorService.Settings CursorSettings => cursorSettings;
        public UiSettings UiSettings => uiSettings;
        public InputBindingsSettings InputBindings => inputBindings;
    }
}
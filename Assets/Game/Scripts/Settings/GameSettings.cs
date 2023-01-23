using Game.Persistence;
using UnityEngine;

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
        private PersistenceService.Settings saveSettings;

        public CameraSettings CameraSettings => cameraSettings;
        public UiSettings UiSettings => uiSettings;
        public LootSettings LootSettings => lootSettings;

        public LevelsSettings LevelsSettings => levelsSettings;

        public PersistenceService.Settings SaveSettings => saveSettings;
    }
}
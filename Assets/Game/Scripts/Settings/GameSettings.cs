using Game.GameFlow;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/Game")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField]
        private InitialSettings initialSettings;

        [SerializeField]
        private CameraSettings cameraSettings;

        public InitialSettings InitialSettings => initialSettings;
        public CameraSettings CameraSettings => cameraSettings;
    }
}
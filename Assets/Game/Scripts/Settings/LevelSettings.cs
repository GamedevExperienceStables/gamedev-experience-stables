using Game.Level;
using Game.Utils.Persistence;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/Level")]
    public class LevelSettings : SerializableScriptableObject
    {
        [SerializeField]
        private LocationDefinition location;

        [SerializeField]
        private LevelGoalsSettings goals;

        public LocationDefinition Location => location;
    }
}
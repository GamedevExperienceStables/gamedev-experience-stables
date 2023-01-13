using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Location Point")]
    public class LocationPointDefinition : ScriptableObject
    {
        [SerializeField]
        private LocationDefinition location;

        [SerializeField]
        private LocationPointKey locationPointKey;

        public LocationDefinition Location => location;
        public LocationPointKey PointKey => locationPointKey;
    }
}
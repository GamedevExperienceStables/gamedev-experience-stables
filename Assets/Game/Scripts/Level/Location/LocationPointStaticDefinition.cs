using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Location/Point Static")]
    public class LocationPointStaticDefinition : LocationPointDefinition
    {
        [SerializeField]
        private LocationDefinition location;

        public LocationDefinition Location => location;
    }
}
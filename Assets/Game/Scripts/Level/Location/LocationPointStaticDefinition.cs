using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Data/Location/Point Static")]
    public class LocationPointStaticDefinition : LocationPointDefinition, ILocationPoint
    {
        [SerializeField]
        private LocationDefinition location;

        public ILocationDefinition Location => location;
    }
}
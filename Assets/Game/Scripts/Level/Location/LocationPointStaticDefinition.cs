using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "★ Location/Point/Static")]
    public class LocationPointStaticDefinition : LocationPointDefinition, ILocationPoint
    {
        [SerializeField]
        private LocationDefinition location;

        public ILocationDefinition Location => location;
    }
}
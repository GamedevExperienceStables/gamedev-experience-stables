using UnityEngine;

namespace Game.UI
{
    [CreateAssetMenu(menuName = "Location/Map Marker Definition")]
    public class MapMarkerDefinition : ScriptableObject
    {
        [SerializeField]
        private Sprite icon;

        public Sprite Icon => icon;
    }
}
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Crystal")]
    public class MaterialDefinition : ItemInventoryDefinition
    {
        [SerializeField]
        private Color color = Color.black;

        [SerializeField]
        private LocalizedString localizedName;

        public Color Color => color;

        public LocalizedString LocalizedName => localizedName;
    }
}
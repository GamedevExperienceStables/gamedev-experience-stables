using UnityEngine;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Crystal")]
    public class MaterialDefinition : ItemInventoryDefinition
    {
        [SerializeField]
        private Color color = Color.black;

        public Color Color => color;
    }
}
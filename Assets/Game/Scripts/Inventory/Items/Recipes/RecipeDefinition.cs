using UnityEngine;

namespace Game.Inventory
{
    [CreateAssetMenu(menuName = MENU_PATH + "Recipe")]
    public class RecipeDefinition : ItemDefinition
    {
        [SerializeField]
        private RuneDefinition grantsRune;
    }
}
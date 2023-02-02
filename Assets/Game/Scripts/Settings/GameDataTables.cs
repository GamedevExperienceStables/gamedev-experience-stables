using Game.Inventory;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/DataTables")]
    public class GameDataTables : ScriptableObject
    {
        [SerializeField]
        private RuneDataTable runes;
        
        [SerializeField]
        private RecipeDataTable recipes;
        
        [SerializeField]
        private MaterialDataTable materials;

        public RuneDataTable Runes => runes;

        public RecipeDataTable Recipes => recipes;

        public MaterialDataTable Materials => materials;
    }
}
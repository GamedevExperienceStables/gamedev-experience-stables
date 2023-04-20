using Game.Inventory;
using Game.Level;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/DataTables")]
    public class GameDataTables : ScriptableObject
    {
        [SerializeField]
        private RuneDataTable runes;
        
        [SerializeField]
        private MaterialDataTable materials;

        [SerializeField]
        private LocationDataTable locations;

        public RuneDataTable Runes => runes;

        public MaterialDataTable Materials => materials;
        
        public LocationDataTable Locations => locations;
    }
}
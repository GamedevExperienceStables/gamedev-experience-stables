using Game.UI;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/FX")]
    public class FxSettings : ScriptableObject
    {
        [SerializeField]
        private CommonFx.Settings common;
        
        [SerializeField]
        private InventoryFx.Settings inventory;
        
        [SerializeField]
        private HudRunesFx.Settings hudRunes;
        
        [SerializeField]
        private HudDamageFx.Settings hudDamage;
        
        [SerializeField]
        private GameOverFx.Settings gameOver;

        public InventoryFx.Settings Inventory => inventory;

        public HudRunesFx.Settings HudRunes => hudRunes;

        public GameOverFx.Settings GameOver => gameOver;

        public CommonFx.Settings Common => common;

        public HudDamageFx.Settings HudDamage => hudDamage;
    }
}
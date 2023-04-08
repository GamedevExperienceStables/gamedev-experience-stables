using Game.Level;
using Game.UI;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/UI")]
    public class UiSettings : ScriptableObject
    {
        [SerializeField]
        private StartMenuView.Settings startMenu;
        
        [SerializeField]
        private LocalizationInteractionSettings interaction;

        public StartMenuView.Settings StartMenu => startMenu;

        public LocalizationInteractionSettings Interaction => interaction;
    }
}
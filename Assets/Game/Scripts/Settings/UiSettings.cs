using Game.UI;
using UnityEngine;

namespace Game.Settings
{
    [CreateAssetMenu(menuName = "Settings/UI")]
    public class UiSettings : ScriptableObject
    {
        [SerializeField]
        private StartMenuView.Settings startMenu;

        public StartMenuView.Settings StartMenu => startMenu;
    }
}
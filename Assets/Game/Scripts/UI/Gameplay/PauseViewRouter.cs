using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseViewRouter : PageRouter
    {
        private PauseMenuView _menu;
        private PauseSettingsView _settings;

        private void Awake()
        {
            _menu = GetComponentInChildren<PauseMenuView>();
            _settings = GetComponentInChildren<PauseSettingsView>();
        }

        private void Start()
        {
            _menu.Hide();
            _settings.Hide();

            OpenMenu();
        }

        public void OpenMenu()
            => Open(_menu);

        public void OpenSettings()
            => Open(_settings);

        public void ToRoot()
            => OpenMenu();
    }
}
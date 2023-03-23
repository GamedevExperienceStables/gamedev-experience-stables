using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.Localization
{
    public sealed class UnityLocalization : ILocalizationService, IDisposable
    {
        private const string GUI_TABLE = "GUI";

        public event Action Changed;

        public UnityLocalization()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        public string GetText(LocalizationTable.GuiKeys key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(GUI_TABLE, key.ToString());
        }

        private void OnLocaleChanged(Locale _)
        {
            Changed?.Invoke();
        }
    }
}
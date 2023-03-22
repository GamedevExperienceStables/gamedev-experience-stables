using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.Localization
{
    public class UnityLocalization : ILocalizationService
    {
        public event Action Changed;

        public UnityLocalization()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        public string GetText(LocalizationTable.GuiKeys guiKeys)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString("GUI", guiKeys.ToString());
        }

        private void OnLocaleChanged(Locale _)
        {
            Changed?.Invoke();
        }
    }
}
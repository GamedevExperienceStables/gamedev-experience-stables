﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public string CurrentLocale => LocalizationSettings.SelectedLocale.LocaleName;

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        public string GetText(LocalizationTable.GuiKeys key)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(GUI_TABLE, key.ToString());
        }

        public void SetLocale(string localeName)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales
                .Find(l => l.LocaleName == localeName);
            LocalizationSettings.SelectedLocale = locale;
        }

        public List<string> GetLocales()
        {
            return LocalizationSettings.AvailableLocales.Locales
                .Select(l => l.LocaleName)
                .ToList();
        }

        private void OnLocaleChanged(Locale _)
        {
            Changed?.Invoke();
        }
    }
}
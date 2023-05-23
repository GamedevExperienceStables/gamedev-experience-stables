using System;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Game.Localization
{
    public sealed class UnityLocalization : ILocalizationService, IDisposable
    {
        public event Action Changed;

        public UnityLocalization()
        {
            LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        }

        public Locale CurrentLocale => LocalizationSettings.SelectedLocale;

        public void Dispose()
        {
            LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        }

        public void SetLocale(Locale locale)
        {
            if (!LocalizationSettings.AvailableLocales.Locales.Contains(locale))
                return;
            
            LocalizationSettings.SelectedLocale = locale;
        }

        public void SetLocale(string localeCode)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales
                .Find(l => l.Identifier.Code == localeCode);
            LocalizationSettings.SelectedLocale = locale;
        }

        public List<Locale> GetLocales() 
            => LocalizationSettings.AvailableLocales.Locales;

        private void OnLocaleChanged(Locale _)
        {
            Changed?.Invoke();
        }
    }
}
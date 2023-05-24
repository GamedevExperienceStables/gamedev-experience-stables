using System.Collections.Generic;
using Game.Localization;
using Game.Persistence;
using UnityEngine.Localization;

namespace Game.Player
{
    public class PlayerLocalizationPrefs
    {
        private const string LOCALE_KEY = "locale";
        
        private readonly ILocalizationService _localization;
        private readonly IPlayerPrefs _playerPrefs;

        public string CurrentLocaleCode => _playerPrefs.GetString(LOCALE_KEY, _localization.CurrentLocale.Identifier.Code);

        public PlayerLocalizationPrefs(ILocalizationService localization, IPlayerPrefs playerPrefs)
        {
            _localization = localization;
            _playerPrefs = playerPrefs;
        }

        public void Init() 
            => _localization.SetLocale(CurrentLocaleCode);

        public void SetLocale(string localeCode)
        {
            _playerPrefs.SetString(LOCALE_KEY, localeCode);
            _localization.SetLocale(localeCode);
        }

        public void SetLocale(Locale locale)
        {
            string localeCode = locale.Identifier.Code;
            _playerPrefs.SetString(LOCALE_KEY, localeCode);
            
            _localization.SetLocale(locale);
        }

        public List<Locale> GetLocales() 
            => _localization.GetLocales();
    }
}
using System.Collections.Generic;
using Game.Localization;
using Game.Persistence;

namespace Game.Player
{
    public class PlayerLocalizationPrefs
    {
        private const string LOCALE_KEY = "locale";
        
        private readonly ILocalizationService _localization;
        private readonly IPlayerPrefs _playerPrefs;

        public string CurrentLocale => _playerPrefs.GetString(LOCALE_KEY, _localization.CurrentLocale);

        public PlayerLocalizationPrefs(ILocalizationService localization, IPlayerPrefs playerPrefs)
        {
            _localization = localization;
            _playerPrefs = playerPrefs;
        }

        public void Init() 
            => _localization.SetLocale(CurrentLocale);

        public void SetLocale(string localeName)
        {
            _playerPrefs.SetString(LOCALE_KEY, localeName);
            _localization.SetLocale(localeName);
        }

        public List<string> GetLocales() 
            => _localization.GetLocales();
    }
}
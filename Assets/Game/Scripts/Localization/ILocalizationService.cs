using System;
using System.Collections.Generic;

namespace Game.Localization
{
    public interface ILocalizationService
    {
        event Action Changed;
        string CurrentLocale { get; }
        void SetLocale(string localeName);
        List<string> GetLocales();
    }
}
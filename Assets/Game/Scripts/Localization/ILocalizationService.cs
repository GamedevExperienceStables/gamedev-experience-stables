using System;
using System.Collections.Generic;
using UnityEngine.Localization;

namespace Game.Localization
{
    public interface ILocalizationService
    {
        event Action Changed;
        Locale CurrentLocale { get; }
        void SetLocale(Locale locale);
        void SetLocale(string localeCode);
        List<Locale> GetLocales();
    }
}
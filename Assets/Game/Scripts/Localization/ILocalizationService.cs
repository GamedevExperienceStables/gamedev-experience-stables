using System;
using System.Collections.Generic;

namespace Game.Localization
{
    public interface ILocalizationService
    {
        event Action Changed;
        string GetText(LocalizationTable.GuiKeys key);
        string CurrentLocale { get; }
        void SetLocale(string localeName);
        List<string> GetLocales();
    }
}
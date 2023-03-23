using System;

namespace Game.Localization
{
    public interface ILocalizationService
    {
        event Action Changed;
        string GetText(LocalizationTable.GuiKeys key);
    }
}
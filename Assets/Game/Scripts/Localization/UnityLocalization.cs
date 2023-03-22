using System;

namespace Game.Localization
{
    public class UnityLocalization : ILocalizationService
    {
        public event Action Changed;

        public UnityLocalization()
        {
            
        }

        public string GetText(LocalizationTable.GuiKeys guiKeys)
        {
            throw new NotImplementedException();
        }
    }
}
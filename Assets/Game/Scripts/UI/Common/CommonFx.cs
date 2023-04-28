using System;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class CommonFx
    {
        private readonly CommonButtonFx _primary;
        private readonly CommonButtonFx _menu;
        private readonly CommonButtonFx _modal;

        public CommonFx(UiFxService uiFx, Settings settings)
        {
            _primary = new CommonButtonFx(uiFx, settings.primary);
            _menu = new CommonButtonFx(uiFx, settings.menu);
            _modal = new CommonButtonFx(uiFx, settings.modal);
        }

        public void RegisterButton(Button button, ButtonStyle style)
        {
            switch (style)
            {
                case ButtonStyle.Primary:
                    _primary.RegisterButton(button);
                    break;

                case ButtonStyle.Menu:
                    _menu.RegisterButton(button);
                    break;
                case ButtonStyle.Modal:
                    _modal.RegisterButton(button);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }

        public void UnRegisterButton(Button button, ButtonStyle style)
        {
            switch (style)
            {
                case ButtonStyle.Primary:
                    _primary.UnRegisterButton(button);
                    break;

                case ButtonStyle.Menu:
                    _menu.UnRegisterButton(button);
                    break;
                case ButtonStyle.Modal:
                    _modal.UnRegisterButton(button);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }


        [Serializable]
        public class Settings
        {
            public CommonButtonFx.Settings primary;
            public CommonButtonFx.Settings menu;
            public CommonButtonFx.Settings modal;
        }
    }
}
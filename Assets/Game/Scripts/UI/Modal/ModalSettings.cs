using System;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public struct ModalSettings
    {
        public LocalizedString title;
        public LocalizedString message;
        public ModalStyle style;
    }

    public static class ModalSettingsExtensions
    {
        public static ModalContext CreateContext(ModalSettings settings, Action onConfirm)
        {
            var context = new ModalContext
            {
                title = settings.title.GetLocalizedString(),
                style = settings.style,
                onConfirm = onConfirm
            };

            if (!settings.message.IsEmpty)
                context.message = settings.message.GetLocalizedString();

            return context;
        }
    }
}
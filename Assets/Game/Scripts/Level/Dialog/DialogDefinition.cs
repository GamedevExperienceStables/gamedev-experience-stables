using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Dialog")]
    public class DialogDefinition : ScriptableObject, IDialogDefinition
    {
        [SerializeField]
        private Item item;

        public IDialogItem Single => item;

        [Serializable]
        public class Item : IDialogItem
        {
            [SerializeField]
            private LocalizedString title;

            [SerializeField]
            private LocalizedString text;

            public string Title => title.IsEmpty ? string.Empty : title.GetLocalizedString();

            public string Text => text.IsEmpty ? string.Empty : text.GetLocalizedString();
        }
    }
    
    
}
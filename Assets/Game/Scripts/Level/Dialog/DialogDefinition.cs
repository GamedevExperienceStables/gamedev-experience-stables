using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Dialog/Dialog")]
    public class DialogDefinition : ScriptableObject, IDialogDefinition
    {
        [SerializeField]
        private Item item;

        public IDialogItem Single => item;

        [Serializable]
        public class Item : IDialogItem
        {
            [SerializeField]
            private DialogActorDefinition actor;
            
            [SerializeField]
            private LocalizedString title;

            [SerializeField]
            private LocalizedString text;

            private bool IsActor => actor;

            public string Title
            {
                get
                {
                    if (IsActor) 
                        return actor.DisplayName.GetLocalizedString();
                    
                    return title.IsEmpty ? string.Empty : title.GetLocalizedString();
                }
            }
            
            public Sprite Image 
                => IsActor ? actor.Image : null;

            public string Text => text.IsEmpty ? string.Empty : text.GetLocalizedString();
        }
    }
    
    
}
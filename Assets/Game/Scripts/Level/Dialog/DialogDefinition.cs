using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Dialog")]
    public class DialogDefinition : ScriptableObject
    {
        [SerializeField]
        private Item item;

        public Item Single => item;

        [Serializable]
        public class Item
        {
            [SerializeField]
            private LocalizedString title;

            [SerializeField]
            private LocalizedString text;

            public LocalizedString Title => title;

            public LocalizedString Text => text;
        }
    }
    
    
}
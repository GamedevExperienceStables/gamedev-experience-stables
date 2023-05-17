using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public class HelpSettings
    {
        [SerializeField]
        public LocalizedString heading;
        [SerializeField]
        public LocalizedString back;
            
        [SerializeField]
        private HelpContentData helpData;
        
        public HelpContentData HelpData => helpData;
    }
}
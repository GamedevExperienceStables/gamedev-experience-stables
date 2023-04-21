using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public class AboutSettings
    {
        [SerializeField]
        private ModalSettings linkModal;
        
        [SerializeField]
        private TeamsSettings teams;
        
        [SerializeField]
        public LocalizedString header;
        [SerializeField]
        public LocalizedString back;

        public ModalSettings LinkModal => linkModal;
        public TeamsSettings Teams => teams;
    }
}
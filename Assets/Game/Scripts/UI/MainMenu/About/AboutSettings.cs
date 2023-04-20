using System;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class AboutSettings
    {
        [SerializeField]
        private ModalSettings linkModal;
        
        [SerializeField]
        private TeamsSettings teams;

        public ModalSettings LinkModal => linkModal;
        public TeamsSettings Teams => teams;
    }
}
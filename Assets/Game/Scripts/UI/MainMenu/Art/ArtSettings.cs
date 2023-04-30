using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Serialization;

namespace Game.UI
{
    [Serializable]
    public class ArtSettings
    {
       
        [SerializeField]
        public LocalizedString header;
        [SerializeField]
        public LocalizedString download;
        [SerializeField]
        public LocalizedString back;

        [SerializeField]
        private ArtBookData artBookData;
        [SerializeField]
        private String downloadLink;
        [SerializeField]
        private ModalSettings downloadModal; 
        
        public ModalSettings DownloadModal => downloadModal;
        public ArtBookData ArtBookData => artBookData;
        public String DownloadLink => downloadLink;
    }
}
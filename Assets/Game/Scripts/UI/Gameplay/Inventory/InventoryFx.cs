using System;
using UnityEngine;

namespace Game.UI
{
    public class InventoryFx
    {
        private readonly Settings _settings;
        private readonly UiFxService _uiFx;

        public InventoryFx(Settings settings, UiFxService uiFx)
        {
            _settings = settings;
            _uiFx = uiFx;
        }

        public void Destroy()
        {
            _uiFx.Destroy(_settings.showFeedbacks);
            _uiFx.Destroy(_settings.hideFeedbacks);
            _uiFx.Destroy(_settings.placeRuneFeedbacks);
            _uiFx.Destroy(_settings.removeRuneFeedbacks);
        }

        public void PlaceRuneFeedback() 
            => _uiFx.Play(_settings.placeRuneFeedbacks);

        public void RemoveRuneFeedback()
            => _uiFx.Play(_settings.removeRuneFeedbacks);

        public void RuneHoverFeedback() 
            => _uiFx.Play(_settings.hoverRuneFeedbacks);

        public void ShowFeedback() 
            => _uiFx.Play(_settings.showFeedbacks);

        public void HideFeedback()
            => _uiFx.Play(_settings.hideFeedbacks);

        [Serializable]
        public class Settings
        {
            public GameObject showFeedbacks;
            public GameObject hideFeedbacks;
            public GameObject placeRuneFeedbacks;
            public GameObject removeRuneFeedbacks;
            public GameObject hoverRuneFeedbacks;
        }
    }
}
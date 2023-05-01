using System;
using UnityEngine;

namespace Game.UI
{
    public class HudRunesFx
    {
        private readonly UiFxService _uiFx;
        private readonly Settings _settings;

        public HudRunesFx(UiFxService uiFx, Settings settings)
        {
            _uiFx = uiFx;
            _settings = settings;
        }

        public void Destroy()
        {
            _uiFx.Destroy(_settings.activateFeedback);
            _uiFx.Destroy(_settings.deactivateFeedback);
        }

        public void ActivateFeedback() 
            => _uiFx.Play(_settings.activateFeedback);

        public void DeactivateFeedback() 
            => _uiFx.Play(_settings.deactivateFeedback);


        [Serializable]
        public class Settings
        {
            public GameObject activateFeedback;
            public GameObject deactivateFeedback;
        }
    }
}
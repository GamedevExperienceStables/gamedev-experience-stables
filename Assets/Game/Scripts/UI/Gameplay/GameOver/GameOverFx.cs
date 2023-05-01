using System;
using UnityEngine;

namespace Game.UI
{
    public class GameOverFx
    {
        private readonly UiFxService _uiFx;
        private readonly Settings _settings;

        public GameOverFx(UiFxService uiFx, Settings settings)
        {
            _uiFx = uiFx;
            _settings = settings;
        }

        public void ShowFeedback()
            => _uiFx.Play(_settings.showFeedback);

        public void Destroy()
            => _uiFx.Destroy(_settings.showFeedback);


        [Serializable]
        public class Settings
        {
            public GameObject showFeedback;
        }
    }
}
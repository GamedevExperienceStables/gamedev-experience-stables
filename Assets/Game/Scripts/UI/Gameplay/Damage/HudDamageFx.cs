﻿using System;
using UnityEngine;

namespace Game.UI
{
    public class HudDamageFx
    {
        private readonly UiFxService _uiFx;
        private readonly Settings _settings;

        public HudDamageFx(UiFxService uiFx, Settings settings)
        {
            _uiFx = uiFx;
            _settings = settings;
        }

        public void DamageFeedback()
            => _uiFx.Play(_settings.damageFeedback);

        public void Destroy()
            => _uiFx.Destroy(_settings.damageFeedback);


        [Serializable]
        public class Settings
        {
            public GameObject damageFeedback;
        }
    }
}
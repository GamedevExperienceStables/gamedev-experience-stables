using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public struct HelpContentData
    {
        public LocalizedString description;
        public LocalizedString header;
        public List<Control> controls;

        [Serializable]
        public struct Control
        {
            public LocalizedString action;
            public String key;
        }
    }
}
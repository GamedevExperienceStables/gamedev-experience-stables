using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.UI
{
    [Serializable]
    public class CutsceneSlide
    {
        [SerializeField]
        private Sprite image;

        [SerializeField]
        private List<LocalizedString> texts;

        public Sprite Image => image;

        public IList<LocalizedString> Texts => texts;
    }
}
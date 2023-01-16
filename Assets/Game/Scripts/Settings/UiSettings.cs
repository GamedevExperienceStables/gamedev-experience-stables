using System;
using Game.UI;
using UnityEngine;

namespace Game.Settings
{
    [Serializable]
    public class UiSettings
    {
        [SerializeField]
        private FaderScreenView fadeScreen;

        [SerializeField]
        private LoadingScreenView loadingScreen;

        public FaderScreenView FadeScreen => fadeScreen;
        public LoadingScreenView LoadingScreen => loadingScreen;
    }
}
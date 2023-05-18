using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.Player
{
    public class PlayerGraphicsPrefs
    {
        public void Init() 
            => Application.backgroundLoadingPriority = ThreadPriority.Low;

        public bool GetFullScreen()
            => Screen.fullScreen;

        public void SetFullscreen(bool value)
            => Screen.fullScreen = value;


        public Resolution GetResolution()
            => Screen.currentResolution;

        public void SetResolution(int width, int height, FullScreenMode mode, RefreshRate refreshRateRatio)
            => Screen.SetResolution(width, height, mode, refreshRateRatio);

        public int GetQualityLevel()
            => QualitySettings.GetQualityLevel();

        public void SetQuality(int index)
            => QualitySettings.SetQualityLevel(index);
    }
}
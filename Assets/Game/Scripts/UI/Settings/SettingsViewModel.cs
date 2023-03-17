using System.Collections.Generic;
using Game.Audio;
using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.UI
{
    public class SettingsViewModel
    {
        private readonly IAudioTuner _audio;

        public SettingsViewModel(IAudioTuner audio) 
            => _audio = audio;

        public bool IsFullscreen => Screen.fullScreen;
        public Resolution CurrentScreenResolution => Screen.currentResolution;
        public int CurrentQuality => QualitySettings.GetQualityLevel();
        public int CurrentScreenHeight => Screen.height;
        public int CurrentScreenWidth => Screen.width;

        public void SetFullscreen(bool value)
            => Screen.fullScreen = value;

        public void SetScreenResolution(Resolution resolution)
        {
            FullScreenMode mode = IsFullscreen
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed;

            Screen.SetResolution(resolution.width, resolution.height, mode, resolution.refreshRateRatio);
        }

        public IList<Resolution> GetScreenResolutions()
            => Screen.resolutions;

        public void SetVolume(AudioChannel channel, float value) 
            => _audio.SetVolume(channel, value * 0.01f);

        public float GetVolume(AudioChannel channel)
            => _audio.GetVolume(channel) * 100;

        public IEnumerable<string> GetQualityNames()
            => QualitySettings.names;

        public void SetQuality(int index)
            => QualitySettings.SetQualityLevel(index);
    }
}
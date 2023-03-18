using System.Collections.Generic;
using Game.Audio;
using Game.Player;
using UnityEngine;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.UI
{
    public class SettingsViewModel
    {
        private readonly PlayerAudioPrefs _audio;
        private readonly PlayerGraphicsPrefs _graphics;

        public SettingsViewModel(PlayerAudioPrefs audio, PlayerGraphicsPrefs graphics)
        {
            _audio = audio;
            _graphics = graphics;
        }

        public bool IsFullscreen => _graphics.GetFullScreen();
        public Resolution CurrentScreenResolution => _graphics.GetResolution();
        public int CurrentQuality => _graphics.GetQualityLevel();
        
        public int CurrentScreenHeight => Screen.height;
        public int CurrentScreenWidth => Screen.width;

        public void SetFullscreen(bool value)
            => _graphics.SetFullscreen(value);

        public void SetScreenResolution(Resolution resolution)
        {
            FullScreenMode mode = IsFullscreen
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed;

            _graphics.SetResolution(resolution.width, resolution.height, mode, resolution.refreshRateRatio);
        }

        public void SetQuality(int index)
            => _graphics.SetQuality(index);

        public IEnumerable<string> GetQualityNames()
            => QualitySettings.names;

        public IList<Resolution> GetScreenResolutions()
            => Screen.resolutions;
        
        public void SetVolume(AudioChannel channel, float value) 
            => _audio.SetVolume(channel, value);

        public float GetVolume(AudioChannel channel)
            => _audio.GetVolume(channel);
    }
}
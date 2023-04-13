using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class SettingsView
    {
        private readonly SettingsViewModel _viewModel;
        private readonly ILocalizationService _localization;
        private Settings _settings;
        
        private Label _graphicLabel;
        private Label _audioLabel;

        private DropdownField _fieldQuality;
        private DropdownField _fieldResolution;
        private Toggle _fieldFullscreen;

        private Slider _fieldMasterVolume;
        private Slider _fieldEffectsVolume;
        private Slider _fieldMusicVolume;

        private IList<Resolution> _resolutions;
        private List<string> _resolutionsOptions;

        private List<string> _qualityOptions;
        
        private DropdownField _fieldLocale;

        public SettingsView(SettingsViewModel viewModel, ILocalizationService localization, Settings settings)
        {
            _viewModel = viewModel;
            _localization = localization;
            _settings = settings;
        }

        public void Create(VisualElement root)
        {
            _graphicLabel = root.Q<Label>("label-graphics");
            _audioLabel = root.Q<Label>("label-audio");
            
            _fieldQuality = root.Q<DropdownField>("field-quality");
            _fieldResolution = root.Q<DropdownField>("field-resolution");
            _fieldFullscreen = root.Q<Toggle>("field-fullscreen");

            _fieldMasterVolume = root.Q<Slider>("field-audio-master");
            _fieldEffectsVolume = root.Q<Slider>("field-audio-effects");
            _fieldMusicVolume = root.Q<Slider>("field-audio-music");
            
            _fieldLocale = root.Q<DropdownField>("field-locale");

            RegisterCallbacks();
        }

        public void Destroy()
            => UnregisterCallbacks();

        public void Init()
        {
            InitQuality();
            InitVolumes();
            InitFullscreen();
            InitScreenResolutions();
            InitLocalization();
            //UpdateText(); Broken
        }

        private void RegisterCallbacks()
        {
            _fieldFullscreen.RegisterValueChangedCallback(OnChangeFullscreen);
            _fieldResolution.RegisterValueChangedCallback(OnChangeResolution);
            _fieldQuality.RegisterValueChangedCallback(OnChangeQuality);

            _fieldMasterVolume.RegisterValueChangedCallback(OnChangeMasterVolume);
            _fieldEffectsVolume.RegisterValueChangedCallback(OnChangeEffectsVolume);
            _fieldMusicVolume.RegisterValueChangedCallback(OnChangeMusicVolume);

            _fieldLocale.RegisterValueChangedCallback(OnChangeLocale);
            
            _localization.Changed += OnLocalisationChanged;
        }

        private void UnregisterCallbacks()
        {
            _fieldFullscreen.UnregisterValueChangedCallback(OnChangeFullscreen);
            _fieldResolution.UnregisterValueChangedCallback(OnChangeResolution);
            _fieldQuality.UnregisterValueChangedCallback(OnChangeQuality);

            _fieldMasterVolume.UnregisterValueChangedCallback(OnChangeMasterVolume);
            _fieldEffectsVolume.UnregisterValueChangedCallback(OnChangeEffectsVolume);
            _fieldMusicVolume.UnregisterValueChangedCallback(OnChangeMusicVolume);
            
            _fieldLocale.UnregisterValueChangedCallback(OnChangeLocale);
            
            _localization.Changed -= OnLocalisationChanged;
        }
        
        private void OnLocalisationChanged()
            => UpdateText();

        private void UpdateText()
        {
            // Broken !
            /*_graphicLabel.text = _settings.graphic.label.GetLocalizedString();
            _audioLabel.text = _settings.audio.label.GetLocalizedString();

            _fieldLocale.Q<Label>().text = _settings.language.label.GetLocalizedString();
            
            _fieldQuality.Q<Label>().text = _settings.quality.label.GetLocalizedString();
            _fieldResolution.Q<Label>().text = _settings.resolution.label.GetLocalizedString();
            _fieldFullscreen.Q<Label>().text = _settings.fullscreen.label.GetLocalizedString();
            
            _fieldMasterVolume.Q<Label>().text = _settings.master.label.GetLocalizedString();
            _fieldMusicVolume.Q<Label>().text = _settings.music.label.GetLocalizedString();
            _fieldEffectsVolume.Q<Label>().text = _settings.effects.label.GetLocalizedString();*/
        }

        #region Graphics

        private void InitFullscreen()
            => _fieldFullscreen.SetValueWithoutNotify(_viewModel.IsFullscreen);

        private void InitScreenResolutions()
        {
            _resolutions = _viewModel.GetScreenResolutions();
            _resolutionsOptions = _resolutions
                .Select(x => x.ToString())
                .ToList();

            _fieldResolution.choices = _resolutionsOptions;

            Resolution current = _viewModel.CurrentScreenResolution;
            if (!_viewModel.IsFullscreen)
            {
                Resolution found = _resolutions
                    .FirstOrDefault(r =>
                        r.height == _viewModel.CurrentScreenHeight
                        && r.width == _viewModel.CurrentScreenWidth
                        && r.refreshRateRatio.Equals(current.refreshRateRatio)
                    );

                if (found.height != 0)
                    current = found;
            }

            int i = _resolutionsOptions.IndexOf(current.ToString());
            if (i >= 0)
                _fieldResolution.SetValueWithoutNotify(current.ToString());
        }

        private void InitQuality()
        {
            _qualityOptions = _viewModel.GetQualityNames().ToList();
            _fieldQuality.choices = _qualityOptions;

            int current = _viewModel.CurrentQuality;
            _fieldQuality.SetValueWithoutNotify(_qualityOptions[current]);
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            int i = _qualityOptions.IndexOf(evt.newValue);
            _viewModel.SetQuality(i);
        }


        private void OnChangeFullscreen(ChangeEvent<bool> evt)
            => _viewModel.SetFullscreen(evt.newValue);

        private void OnChangeResolution(ChangeEvent<string> evt)
        {
            int i = _resolutionsOptions.IndexOf(evt.newValue);
            Resolution resolution = _resolutions[i];

            _viewModel.SetScreenResolution(resolution);
        }

        #endregion

        #region Audio

        private void InitVolumes()
        {
            _fieldMasterVolume.SetValueWithoutNotify(GetVolume(AudioChannel.Master));
            _fieldMusicVolume.SetValueWithoutNotify(GetVolume(AudioChannel.Music));
            _fieldEffectsVolume.SetValueWithoutNotify(GetVolume(AudioChannel.Effects));
        }

        private void OnChangeMasterVolume(ChangeEvent<float> evt)
            => SetVolume(AudioChannel.Master, evt.newValue);

        private void OnChangeEffectsVolume(ChangeEvent<float> evt)
            => SetVolume(AudioChannel.Effects, evt.newValue);

        private void OnChangeMusicVolume(ChangeEvent<float> evt)
            => SetVolume(AudioChannel.Music, evt.newValue);

        private float GetVolume(AudioChannel audioChannel)
        {
            float convertedToFieldVolume = _viewModel.GetVolume(audioChannel) * 100;
            return convertedToFieldVolume;
        }

        private void SetVolume(AudioChannel audioChannel, float value)
        {
            float convertedFromFieldVolume = value * 0.01f;
            _viewModel.SetVolume(audioChannel, convertedFromFieldVolume);
        }

        #endregion

        #region Localization
        
        private void InitLocalization()
        {
            _fieldLocale.choices = _viewModel.GetLocales();

            string current = _viewModel.CurrentLocale;
            _fieldLocale.SetValueWithoutNotify(current);
        }

        private void OnChangeLocale(ChangeEvent<string> evt) 
            => _viewModel.SetLocale(evt.newValue);

        #endregion
        
        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString label;
            }

            public Page language;
            public Page graphic;
            public Page quality;
            public Page resolution;
            public Page fullscreen;
            public Page audio;
            public Page master;
            public Page music;
            public Page effects;
        }
    }
}
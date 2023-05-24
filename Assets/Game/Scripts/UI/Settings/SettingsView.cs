using System;
using System.Collections.Generic;
using System.Linq;
using Game.Audio;
using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class SettingsView
    {
        private readonly SettingsViewModel _viewModel;
        private readonly ILocalizationService _localization;
        private readonly Settings _settings;

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

        private readonly List<string> _qualityOptions = new();

        private DropdownField _fieldLocale;
        private List<Locale> _locales;

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

            UpdateText();
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
        {
            InitQuality();
            UpdateText();
        }

        private void UpdateText()
        {
            _graphicLabel.text = _settings.graphic.label.GetLocalizedString();
            _audioLabel.text = _settings.audio.label.GetLocalizedString();

            _fieldLocale.label = _settings.language.label.GetLocalizedString();

            _fieldQuality.label = _settings.quality.label.GetLocalizedString();
            _fieldResolution.label = _settings.resolution.label.GetLocalizedString();
            _fieldFullscreen.label = _settings.fullscreen.label.GetLocalizedString();

            _fieldMasterVolume.label = _settings.master.label.GetLocalizedString();
            _fieldMusicVolume.label = _settings.music.label.GetLocalizedString();
            _fieldEffectsVolume.label = _settings.effects.label.GetLocalizedString();
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
            _qualityOptions.Clear();

            foreach (string qualityName in _viewModel.GetQualityNames())
            {
                string name = qualityName;

                foreach (Settings.QualitySettings.Quality quality in _settings.quality.qualities)
                {
                    if (quality.name != name) 
                        continue;
                    
                    name = quality.label.GetLocalizedString();
                }
                
                _qualityOptions.Add(name);
            }
            
            _fieldQuality.choices = _qualityOptions;

            int current = _viewModel.CurrentQuality;
            _fieldQuality.SetValueWithoutNotify(_qualityOptions[current]);
        }

        private void OnChangeQuality(ChangeEvent<string> evt)
        {
            int i = _qualityOptions.IndexOf(evt.newValue);
            if (i < 0)
                return;

            _viewModel.SetQuality(i);
        }


        private void OnChangeFullscreen(ChangeEvent<bool> evt)
            => _viewModel.SetFullscreen(evt.newValue);

        private void OnChangeResolution(ChangeEvent<string> evt)
        {
            int i = _resolutionsOptions.IndexOf(evt.newValue);
            if (i < 0)
                return;

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
            _locales = _viewModel.GetLocales();
            _fieldLocale.choices = _locales.Select(locale =>
            {
                string nativeName = locale.Identifier.CultureInfo.NativeName;
                nativeName = nativeName.UppercaseFirst();
                return nativeName;
            }).ToList();

            string current = _viewModel.CurrentLocaleCode;
            Locale found = _locales.Find(locale => locale.Identifier.Code == current);
            int index = found ? _locales.IndexOf(found) : 0;
            
            current = _fieldLocale.choices[index];
            _fieldLocale.SetValueWithoutNotify(current);
        }

        private void OnChangeLocale(ChangeEvent<string> evt)
        {
            int index = _fieldLocale.choices.IndexOf(evt.newValue);
            if (index < 0)
                return;
            
            Locale locale = _locales[index];
            _viewModel.SetLocale(locale);
        }

        #endregion

        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString label;
            }
            
            [Serializable]
            public struct QualitySettings
            {
                public LocalizedString label;
                public List<Quality> qualities;
                
                [Serializable]
                public struct Quality
                {
                    public string name;
                    public LocalizedString label;
                }
            }

            public Page language;
            public Page graphic;
            public QualitySettings quality;
            public Page resolution;
            public Page fullscreen;
            public Page audio;
            public Page master;
            public Page music;
            public Page effects;
        }
    }
}
﻿using System;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class PauseMenuView : PageView<PauseMenuViewModel>
    {
        private VisualElement _container;

        private Button _buttonResume;
        private Button _buttonMainMenu;
        private Button _buttonSettings;
        private Label _heading;

        private PauseViewRouter _router;

        private Settings _settings;

        private CommonFx _commonFx;

        [Inject]
        public void Construct(Settings settings, CommonFx commonFx)
        {
            _settings = settings;
            _commonFx = commonFx;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.PauseMenu.CONTAINER);

            _router = GetComponent<PauseViewRouter>();

            _buttonResume = Content.Q<VisualElement>(LayoutNames.PauseMenu.BUTTON_RESUME).Q<Button>();
            _buttonSettings = Content.Q<VisualElement>(LayoutNames.PauseMenu.BUTTON_SETTINGS).Q<Button>();
            _buttonMainMenu = Content.Q<VisualElement>(LayoutNames.PauseMenu.BUTTON_MAIN_MENU).Q<Button>();

            _heading = Content.Q<Label>(LayoutNames.PauseMenu.PAGE_HEADING);

            _buttonResume.clicked += OnResumeButton;
            _buttonMainMenu.clicked += OnMainMenuButton;
            _buttonSettings.clicked += OnSettingsButton;

            _commonFx.RegisterButton(_buttonResume, ButtonStyle.Menu);
            _commonFx.RegisterButton(_buttonSettings, ButtonStyle.Menu);
            _commonFx.RegisterButton(_buttonMainMenu, ButtonStyle.Menu);

            localization.Changed += OnLocalisationChanged;
        }

        private void Start()
            => UpdateText();

        public void OnDestroy()
        {
            _buttonResume.clicked -= OnResumeButton;
            _buttonMainMenu.clicked -= OnMainMenuButton;
            _buttonSettings.clicked -= OnSettingsButton;

            _commonFx.UnRegisterButton(_buttonResume, ButtonStyle.Menu);
            _commonFx.UnRegisterButton(_buttonSettings, ButtonStyle.Menu);
            _commonFx.UnRegisterButton(_buttonMainMenu, ButtonStyle.Menu);

            localization.Changed -= OnLocalisationChanged;
        }

        private void OnLocalisationChanged()
            => UpdateText();

        private void OnResumeButton()
            => ViewModel.ResumeGame();

        private void OnMainMenuButton()
            => ShowMainMenuModal();

        private void ShowMainMenuModal()
        {
            ModalContext context =
                ModalSettingsExtensions.CreateContext(_settings.mainMenuModal, ViewModel.GoToMainMenu);

            ViewModel.ShowModal(context);
        }

        private void OnSettingsButton()
            => _router.OpenSettings();

        private void UpdateText()
        {
            _buttonResume.Q<Label>().text = _settings.resume.button.GetLocalizedString();
            _buttonSettings.Q<Label>().text = _settings.settings.button.GetLocalizedString();
            _buttonMainMenu.Q<Label>().text = _settings.mainMenu.button.GetLocalizedString();
            _heading.text = _settings.heading.GetLocalizedString();
        }

        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString button;
            }

            public Page resume;
            public Page settings;
            public Page mainMenu;

            public LocalizedString heading;

            public ModalSettings mainMenuModal;
        }
    }
}
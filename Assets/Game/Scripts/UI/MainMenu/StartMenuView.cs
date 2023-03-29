using System;
using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class StartMenuView : PageView<StartMenuViewModel>
    {
        private Button _buttonNewGame;
        private Button _buttonContinue;
        private Button _buttonSettings;
        private Button _buttonArt;
        private Button _buttonAbout;
        private Button _buttonQuit;

        private ILocalizationService _localization;
        private PreviewView _preview;
        private VisualElement _menu;

        private Settings _settings;

        [Inject]
        public void Construct(ILocalizationService localisation, Settings settings)
        {
            _localization = localisation;
            _settings = settings;
        }

        protected override void OnAwake()
        {
            _buttonNewGame = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_NEW_GAME).Q<Button>();
            _buttonContinue = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_CONTINUE).Q<Button>();

            _buttonSettings = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_SETTINGS).Q<Button>();
            _buttonAbout = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ABOUT).Q<Button>();
            _buttonArt = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ART).Q<Button>();
            _buttonQuit = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_QUIT).Q<Button>();

            _menu = Content.Q<VisualElement>(LayoutNames.StartMenu.MENU);

            var previewElement = Content.Q<VisualElement>(LayoutNames.StartMenu.PREVIEW);
            _preview = new PreviewView(previewElement);

            RegisterButtonEvent(_buttonNewGame, NewGame, OnEnterNewGame, OnExit);
            RegisterButtonEvent(_buttonContinue, ContinueGame, OnEnter, OnExit);
            RegisterButtonEvent(_buttonSettings, OpenSettings, OnEnterSettings, OnExit);
            RegisterButtonEvent(_buttonArt, OpenArt, OnEnter, OnExit);
            RegisterButtonEvent(_buttonAbout, OpenAbout, OnEnter, OnExit);
            RegisterButtonEvent(_buttonQuit, QuitGame, OnEnter, OnExit);

            _localization.Changed += OnLocalisationChanged;
        }

        private void Start()
            => UpdateText();

        private void OnDestroy()
        {
            UnregisterButtonEvent(_buttonNewGame, NewGame, OnEnter, OnExit);
            UnregisterButtonEvent(_buttonContinue, ContinueGame, OnEnter, OnExit);
            UnregisterButtonEvent(_buttonSettings, OpenSettings, OnEnter, OnExit);
            UnregisterButtonEvent(_buttonArt, OpenArt, OnEnter, OnExit);
            UnregisterButtonEvent(_buttonAbout, OpenAbout, OnEnter, OnExit);
            UnregisterButtonEvent(_buttonQuit, QuitGame, OnEnter, OnExit);

            _localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            _buttonContinue.SetEnabled(ViewModel.IsSaveGameExists());
            _preview.Hide();

            Content.SetDisplay(true);

            _buttonNewGame.Focus();
        }

        public override void Hide()
            => Content.SetDisplay(false);

        public void Activate()
        {
            _menu.SetEnabled(true);

            _buttonSettings.RemoveFromClassList(LayoutNames.StartMenu.BUTTON_ACTIVE_CLASS_NAME);
        }

        public void Deactivate()
        {
            _preview.Hide();
            _menu.SetEnabled(false);
        }

        private void UpdateText()
        {
            _buttonNewGame.Q<Label>().text = _settings.newGameButton.GetLocalizedString();
            _buttonContinue.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Continue_Button);

            _buttonSettings.Q<Label>().text = _settings.settingsButton.GetLocalizedString();
            _buttonArt.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Artbook_Button);
            _buttonAbout.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_About_Button);
            _buttonQuit.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Quit_Button);
        }

        private void NewGame()
            => ViewModel.NewGame();

        private void ContinueGame()
            => ViewModel.ContinueGame();

        private void QuitGame()
            => ViewModel.QuitGame();

        // TODO: Refactor this in next build
        private void OpenArt()
            => Application.OpenURL(
                "https://drive.google.com/file/d/1j8xxQQ9xu0Hz4JW1MmNpHTwj4yJnndEe/view?usp=share_link");

        private void OpenAbout()
            => ViewModel.OpenAbout();

        private void OpenSettings()
        {
            ViewModel.OpenSettings();

            _buttonSettings.AddToClassList(LayoutNames.StartMenu.BUTTON_ACTIVE_CLASS_NAME);
        }

        private void OnLocalisationChanged()
            => UpdateText();

        #region OnPointerOverEvents

        private void OnEnterNewGame(EventBase _)
            => OnEnter(_settings.newGamePreview, _settings.newGameCaption);

        private void OnEnterSettings(EventBase _)
            => OnEnter(_settings.settingsPreview, _settings.settingsCaption);

        private void OnEnter(Sprite preview, LocalizedString caption)
        {
            if (!_menu.enabledSelf)
                return;

            _preview.Show(preview, caption.GetLocalizedString());
        }

        private void OnEnter(EventBase _)
        {
            if (!_menu.enabledSelf)
                return;
            
            _preview.Show();
        }

        #endregion

        private void OnExit(EventBase _)
        {
            if (!_menu.enabledSelf)
                return;

            _preview.Hide();
        }

        private static void RegisterButtonEvent(Button button, Action onClick,
            EventCallback<EventBase> onEnter, EventCallback<EventBase> onLeave)
        {
            button.clicked += onClick;
            
            button.RegisterCallback<FocusInEvent>(onEnter);
            button.RegisterCallback<FocusOutEvent>(onLeave);
            
            button.RegisterCallback<PointerEnterEvent>(onEnter);
            button.RegisterCallback<PointerLeaveEvent>(onEnter);
        }

        private static void UnregisterButtonEvent(Button button, Action onClick,
            EventCallback<EventBase> onEnter, EventCallback<EventBase> onLeave)
        {
            button.clicked -= onClick;
            
            button.UnregisterCallback<FocusInEvent>(onEnter);
            button.UnregisterCallback<FocusOutEvent>(onLeave);
            
            button.UnregisterCallback<PointerEnterEvent>(onEnter);
            button.UnregisterCallback<PointerLeaveEvent>(onLeave);
        }

        [Serializable]
        public class Settings
        {
            [Header("New Game")]
            public Sprite newGamePreview;

            public LocalizedString newGameButton;
            public LocalizedString newGameCaption;

            [Header("Settings")]
            public Sprite settingsPreview;

            public LocalizedString settingsButton;
            public LocalizedString settingsCaption;
        }
    }
}
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

        private VisualElement _bookmark;

        private PreviewView _preview;
        private VisualElement _menu;

        private Settings _settings;
        
        private CommonFx _commonFx;

        private string _url;

        [Inject]
        public void Construct(Settings settings, CommonFx commonFx)
        {
            _settings = settings;
            _commonFx = commonFx;
        }

        protected override void OnAwake()
        {
            SetContent(LayoutNames.StartMenu.PAGE_BOOK);
            
            _buttonNewGame = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_NEW_GAME).Q<Button>();
            _buttonContinue = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_CONTINUE).Q<Button>();

            _buttonSettings = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_SETTINGS).Q<Button>();
            _buttonAbout = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ABOUT).Q<Button>();
            _buttonArt = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ART).Q<Button>();
            _buttonQuit = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_QUIT).Q<Button>();

            _bookmark = Content.Q<VisualElement>(LayoutNames.StartMenu.BOOKMARK);
            _url = _settings.notionLink;

            _menu = Content.Q<VisualElement>(LayoutNames.StartMenu.MENU);

            var previewElement = Content.Q<VisualElement>(LayoutNames.StartMenu.PREVIEW);
            _preview = new PreviewView(previewElement);

            RegisterButtonEvent(_buttonNewGame, NewGame, OnHoverNewGame, OnExit);
            RegisterButtonEvent(_buttonContinue, ContinueGame, OnHoverContinueGame, OnExit);
            RegisterButtonEvent(_buttonSettings, OpenSettings, OnHoverSettings, OnExit);
            RegisterButtonEvent(_buttonArt, OpenArt, OnHoverArt, OnExit);
            RegisterButtonEvent(_buttonAbout, OpenAbout, OnHoverAbout, OnExit);
            RegisterButtonEvent(_buttonQuit, QuitGame, OnHoverQuit, OnExit);
            
            _bookmark.RegisterCallback<ClickEvent>(OnClickBookmark);

            localization.Changed += OnLocalisationChanged;
        }

        private void Start()
            => UpdateText();

        private void OnDestroy()
        {
            UnregisterButtonEvent(_buttonNewGame, NewGame, OnHoverNewGame, OnExit);
            UnregisterButtonEvent(_buttonContinue, ContinueGame, OnHoverContinueGame, OnExit);
            UnregisterButtonEvent(_buttonSettings, OpenSettings, OnHoverSettings, OnExit);
            UnregisterButtonEvent(_buttonArt, OpenArt, OnHoverArt, OnExit);
            UnregisterButtonEvent(_buttonAbout, OpenAbout, OnHoverAbout, OnExit);
            UnregisterButtonEvent(_buttonQuit, QuitGame, OnHoverQuit, OnExit);
            
            _bookmark.UnregisterCallback<ClickEvent>(OnClickBookmark);

            localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            _buttonContinue.SetDisplay(ViewModel.IsSaveGameExists());
            ShowPreview();

            Content.SetEnabled(true);
            Content.RemoveFromClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
        }

        public override void Hide()
        {
            Content.SetEnabled(false);
            Content.AddToClassList(LayoutNames.StartMenu.PAGE_HIDDEN_CLASS_NAME);
        }

        public void Activate()
        {
            _menu.SetEnabled(true);

            _buttonSettings.RemoveFromClassList(LayoutNames.StartMenu.BUTTON_ACTIVE_CLASS_NAME);
            _buttonArt.RemoveFromClassList(LayoutNames.StartMenu.BUTTON_ACTIVE_CLASS_NAME);
            _buttonAbout.RemoveFromClassList(LayoutNames.StartMenu.BUTTON_ACTIVE_CLASS_NAME);
        }

        public void Deactivate()
        {
            _preview.Hide();
            _menu.SetEnabled(false);
        }

        private void OnLocalisationChanged()
            => UpdateText();

        private void UpdateText()
        {
            _buttonNewGame.Q<Label>().text = _settings.newGame.button.GetLocalizedString();
            _buttonContinue.Q<Label>().text = _settings.continueGame.button.GetLocalizedString();

            _buttonSettings.Q<Label>().text = _settings.settings.button.GetLocalizedString();
            _buttonArt.Q<Label>().text = _settings.art.button.GetLocalizedString();
            _buttonAbout.Q<Label>().text = _settings.about.button.GetLocalizedString();
            _buttonQuit.Q<Label>().text = _settings.quit.button.GetLocalizedString();
        }

        private void NewGame()
        {
            if (ViewModel.IsSaveGameExists())
                ShowNewGameModal();
            else
                ViewModel.NewGame();
        }

        private void ContinueGame()
            => ViewModel.ContinueGame();

        private void QuitGame()
            => ShowQuitGameModal();
        
        private void OnClickBookmark(EventBase _)
            => ShowNotionModal();

        private void ShowQuitGameModal()
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(_settings.quitModal, ViewModel.QuitGame);
            ViewModel.ShowModal(context);
        }

        private void ShowNewGameModal()
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(_settings.newGameModal, ViewModel.NewGame);
            ViewModel.ShowModal(context);
        }
        
        private void ShowNotionModal()
        {
            ModalContext context = ModalSettingsExtensions.CreateContext(_settings.notionModal, OpenLink);
            context.message = _url;
            ViewModel.ShowModal(context);
        }
        
        private void OpenLink()
            => ViewModel.OpenURL(_url);

        private void OpenArt()
        {
            ViewModel.OpenArt();

            _buttonArt.AddToClassList(LayoutNames.StartMenu.BUTTON_ART);
        }

        private void OpenAbout() 
            => ViewModel.OpenAbout();

        private void OpenSettings() 
            => ViewModel.OpenSettings();

        #region OnPointerOverEvents

        private void OnHoverNewGame(EventBase _)
            => OnEnter(_settings.newGame.preview, _settings.newGame.caption);

        private void OnHoverContinueGame(EventBase _)
            => OnEnter(_settings.continueGame.preview, _settings.continueGame.caption);

        private void OnHoverSettings(EventBase _)
            => OnEnter(_settings.settings.preview, _settings.settings.caption);

        private void OnHoverArt(EventBase _)
            => OnEnter(_settings.art.preview, _settings.art.caption);

        private void OnHoverAbout(EventBase _)
            => OnEnter(_settings.about.preview, _settings.about.caption);

        private void OnHoverQuit(EventBase _)
            => OnEnter(_settings.quit.preview, _settings.quit.caption);

        private void OnEnter(Sprite preview, LocalizedString caption)
        {
            if (!_menu.enabledInHierarchy)
                return;

            _preview.Show(preview, caption.GetLocalizedString());
        }

        private void OnExit(EventBase _)
        {
            if (!_menu.enabledInHierarchy)
                return;

            ShowPreview();
        }

        #endregion

        private void ShowPreview()
            => _preview.Show(_settings.preview.preview, _settings.preview.caption.GetLocalizedString());

        private void RegisterButtonEvent(Button button, Action onClick,
            EventCallback<EventBase> onEnter, EventCallback<EventBase> onLeave)
        {
            button.clicked += onClick;

            button.RegisterCallback<FocusInEvent>(onEnter);
            button.RegisterCallback<FocusOutEvent>(onLeave);

            button.RegisterCallback<PointerEnterEvent>(onEnter);
            button.RegisterCallback<PointerLeaveEvent>(onLeave);
            
            
            _commonFx.RegisterButton(button, ButtonStyle.Menu);
        }

        private void UnregisterButtonEvent(Button button, Action onClick,
            EventCallback<EventBase> onEnter, EventCallback<EventBase> onLeave)
        {
            button.clicked -= onClick;

            button.UnregisterCallback<FocusInEvent>(onEnter);
            button.UnregisterCallback<FocusOutEvent>(onLeave);

            button.UnregisterCallback<PointerEnterEvent>(onEnter);
            button.UnregisterCallback<PointerLeaveEvent>(onLeave);
            
            _commonFx.UnRegisterButton(button, ButtonStyle.Menu);
        }

        [Serializable]
        public class Settings
        {
            [Serializable]
            public struct Page
            {
                public LocalizedString button;
                public LocalizedString caption;
                public Sprite preview;
            }

            public Page preview;
            public Page newGame;
            public Page continueGame;
            public Page art;
            public Page about;
            public Page settings;
            public Page quit;

            public string notionLink;

            public ModalSettings newGameModal;
            public ModalSettings quitModal;
            public ModalSettings notionModal;
        }
    }
}
using System.Linq;
using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class StartMenuView : PageView<StartMenuViewModel>
    {
        private VisualElement _buttonStart;
        private VisualElement _buttonContinue;
        private VisualElement _buttonSettings;
        private VisualElement _buttonArt;
        private VisualElement _buttonAbout;
        private VisualElement _buttonQuit;
        
        private ILocalizationService _localization;
        
        [Inject]
        public void Construct(ILocalizationService localisation)
        {
            _localization = localisation;
        }

        protected override void OnAwake()
        {
            _buttonStart = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_START);
            _buttonContinue = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_CONTINUE);

            _buttonSettings = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_SETTINGS);
            _buttonAbout = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ABOUT);
            _buttonArt = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_ART);
            _buttonQuit = Content.Q<VisualElement>(LayoutNames.StartMenu.BUTTON_QUIT);

            _buttonStart.RegisterCallback<ClickEvent>(NewGame);
            _buttonContinue.RegisterCallback<ClickEvent>(ContinueGame);

            _buttonSettings.RegisterCallback<ClickEvent>(OpenSettings);
            _buttonArt.RegisterCallback<ClickEvent>(OpenArt);
            _buttonAbout.RegisterCallback<ClickEvent>(OpenAbout);
            _buttonQuit.RegisterCallback<ClickEvent>(QuitGame);
            
            _localization.Changed += OnLocalisationChanged;
        }

        private void Start() 
            => UpdateText();

        private void OnDestroy()
        {
            _buttonStart.UnregisterCallback<ClickEvent>(NewGame);
            _buttonContinue.UnregisterCallback<ClickEvent>(ContinueGame);

            _buttonSettings.UnregisterCallback<ClickEvent>(OpenSettings);
            _buttonArt.UnregisterCallback<ClickEvent>(OpenArt);
            _buttonAbout.UnregisterCallback<ClickEvent>(OpenAbout);
            _buttonQuit.UnregisterCallback<ClickEvent>(QuitGame);
            
            _localization.Changed -= OnLocalisationChanged;
        }

        public override void Show()
        {
            _buttonContinue.SetDisplay(ViewModel.IsSaveGameExists());

            Content.SetDisplay(true);

            _buttonStart.Focus();
        }

        public override void Hide()
        {
            Content.SetDisplay(false);
        }
        
        private void UpdateText()
        {
            _buttonStart.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_NewGame_Button);
            _buttonContinue.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Continue_Button);
            
            _buttonSettings.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Settings_Button);
            _buttonArt.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Artbook_Button);
            _buttonAbout.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_About_Button);
            _buttonQuit.Q<Label>().text = _localization.GetText(LocalizationTable.GuiKeys.Menu_Quit_Button);
        }

        private void NewGame(ClickEvent _)
            => ViewModel.NewGame();

        private void ContinueGame(ClickEvent _)
            => ViewModel.ContinueGame();

        private void QuitGame(ClickEvent _)
            => ViewModel.QuitGame();

        // TODO: Refactor this in next build
        private void OpenArt(ClickEvent _)
            => Application.OpenURL(
                "https://drive.google.com/file/d/1j8xxQQ9xu0Hz4JW1MmNpHTwj4yJnndEe/view?usp=share_link");

        private void OpenAbout(ClickEvent _)
            => ViewModel.OpenAbout();

        private void OpenSettings(ClickEvent _)
            => ViewModel.OpenSettings();
        
        private void OnLocalisationChanged() 
            => UpdateText();
    }
}
using System;
using Game.Localization;
using Game.Localization;
using Game.Utils;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class StartMenuView : PageView<StartMenuViewModel>
    {
        private Button _buttonStart;
        private Button _buttonContinue;
        private Button _buttonQuit;
        private Button _buttonSettings;
        private Button _buttonAbout;
        private Button _buttonArt;
        
        private ILocalizationService _localisation;
        
        [Inject]
        public void Construct(StartMenuViewModel viewModel, ILocalizationService localisation)
        {
            _localisation = localisation;
        }

        protected override void OnAwake()
        {
            _buttonStart = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_START);
            _buttonContinue = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_CONTINUE);

            _buttonSettings = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_SETTINGS);
            _buttonAbout = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_ABOUT);
            _buttonArt = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_ART);
            _buttonQuit = Content.Q<Button>(LayoutNames.StartMenu.BUTTON_QUIT);

            _buttonStart.clicked += NewGame;
            _buttonContinue.clicked += ContinueGame;

            _buttonSettings.clicked += OpenSettings;
            _buttonAbout.clicked += OpenAbout;
            _buttonArt.clicked += OpenArt;
            _buttonQuit.clicked += QuitGame;
            
            _localisation.Changed += OnLocalisationChanged;
        }

        private void Start()
        {
            Show();
            UpdateText();
        }

        private void OnDestroy()
        {
            _buttonStart.clicked -= NewGame;
            _buttonQuit.clicked -= QuitGame;

            _buttonSettings.clicked -= OpenSettings;
            _buttonAbout.clicked -= OpenAbout;
            _buttonArt.clicked -= OpenArt;
            _buttonQuit.clicked -= QuitGame;
            
            _localisation.Changed += OnLocalisationChanged;
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
            _buttonStart.text = _localisation.GetText(LocalizationTable.GuiKeys.New_Game_Button);
            _buttonContinue.text = _localisation.GetText(LocalizationTable.GuiKeys.Continue_Button);
            _buttonQuit.text = _localisation.GetText(LocalizationTable.GuiKeys.Quit_Button);
        }

        private void NewGame()
            => ViewModel.NewGame();

        private void ContinueGame()
            => ViewModel.ContinueGame();

        private void QuitGame()
            => ViewModel.QuitGame();

        private void OpenArt()
            => ViewModel.OpenArt();

        private void OpenAbout()
            => ViewModel.OpenAbout();

        private void OpenSettings()
            => ViewModel.OpenSettings();
    }
}
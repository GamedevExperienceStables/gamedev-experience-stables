using System;
using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class StartMenuView : MonoBehaviour
    {
        private VisualElement _root;

        private Button _buttonStart;
        private Button _buttonContinue;
        private Button _buttonQuit;

        private StartMenuViewModel _viewModel;
        private ILocalizationService _localisation;

        [Inject]
        public void Construct(StartMenuViewModel viewModel, ILocalizationService localisation)
        {
            _viewModel = viewModel;
            _localisation = localisation;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonStart = _root.Q<Button>(LayoutNames.StartMenu.BUTTON_START);
            _buttonStart.clicked += NewGame;
            
            _buttonContinue = _root.Q<Button>(LayoutNames.StartMenu.BUTTON_CONTINUE);
            _buttonContinue.clicked += ContinueGame;

            _buttonQuit = _root.Q<Button>(LayoutNames.StartMenu.BUTTON_QUIT);
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
            _localisation.Changed += OnLocalisationChanged;
        }

        public void Show()
        {
            _buttonContinue.SetDisplay(_viewModel.IsSaveGameExists());

            _root.style.display = DisplayStyle.Flex;

            _buttonStart.Focus();
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }

        private void UpdateText()
        {
            _buttonStart.text = _localisation.GetText(LocalizationTable.GuiKeys.New_Game_Button);
            _buttonContinue.text = _localisation.GetText(LocalizationTable.GuiKeys.Continue_Button);
            _buttonQuit.text = _localisation.GetText(LocalizationTable.GuiKeys.Quit_Button);
        }

        private void NewGame() 
            => _viewModel.NewGame();
        
        private void ContinueGame() 
            => _viewModel.ContinueGame();

        private void QuitGame() 
            => _viewModel.QuitGame();

        private void OnLocalisationChanged() 
            => UpdateText();
    }
}
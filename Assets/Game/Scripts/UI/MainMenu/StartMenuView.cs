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

        [Inject]
        public void Construct(StartMenuViewModel viewModel)
        {
            _viewModel = viewModel;
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
        }

        private void Start() => Show();

        private void OnDestroy()
        {
            _buttonStart.clicked -= NewGame;
            _buttonQuit.clicked -= QuitGame;
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

        private void NewGame() 
            => _viewModel.NewGame();
        
        private void ContinueGame() 
            => _viewModel.ContinueGame();

        private void QuitGame() 
            => _viewModel.QuitGame();
    }
}
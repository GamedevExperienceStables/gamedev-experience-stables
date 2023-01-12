using Game.Level;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseMenuView : MonoBehaviour
    {
        private Button _buttonResume;
        private Button _buttonMainMenu;

        private VisualElement _root;
        private LocationController _level;

        private GameplayViewModel _viewModel;

        [Inject]
        public void Construct(GameplayViewModel viewModel)
            => _viewModel = viewModel;

        public void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonResume = _root.Q<Button>(LayoutNames.PauseMenu.BUTTON_RESUME);
            _buttonMainMenu = _root.Q<Button>(LayoutNames.PauseMenu.BUTTON_MAIN_MENU);

            _buttonResume.clicked += ResumeGame;
            _buttonMainMenu.clicked += GoToMainMenu;
        }

        public void OnDestroy()
        {
            _buttonResume.clicked -= ResumeGame;
            _buttonMainMenu.clicked -= GoToMainMenu;
        }

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;

            _buttonResume.Focus();
        }

        public void Hide()
            => _root.style.display = DisplayStyle.None;

        private void ResumeGame() => _viewModel.ResumeGame();
        private void GoToMainMenu() => _viewModel.GoToMainMenu();
    }
}
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class HudView : MonoBehaviour
    {
        private VisualElement _root;
        private Button _buttonMenu;
        private GameplayViewModel _viewModel;

        [Inject]
        public void Construct(GameplayViewModel viewModel) 
            => _viewModel = viewModel;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonMenu = _root.Q<Button>(LayoutNames.Hud.BUTTON_MENU);
            _buttonMenu.clicked += PauseGame;
        }

        private void OnDestroy()
        {
            _buttonMenu.clicked -= PauseGame;
        }

        private void PauseGame() 
            => _viewModel.PauseGame();

        public void Show() 
            => _root.style.display = DisplayStyle.Flex;

        public void Hide() 
            => _root.style.display = DisplayStyle.None;
    }
}
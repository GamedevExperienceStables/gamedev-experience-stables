using Game.Utils;
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

        public void HideImmediate() 
            => _root.SetDisplay(false);

        public void Show()
            => _root.SetDisplay(true);

        public void Hide() 
            => _root.SetDisplay(false);

        private void PauseGame() 
            => _viewModel.PauseGame();
    }
}
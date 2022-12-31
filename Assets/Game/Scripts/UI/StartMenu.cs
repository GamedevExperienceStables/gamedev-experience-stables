using Game.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class StartMenu : MonoBehaviour
    {
        private VisualElement _root;
        private Button _buttonStart;
        private GameManager _game;

        public void Construct(GameManager game)
        {
            _game = game;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            
            _buttonStart = _root.Q<Button>(LayoutNames.StartMenu.BUTTON_START);
            _buttonStart.clicked += OnStartButtonClicked;
        }

        private void OnDestroy()
        {
            _buttonStart.clicked -= OnStartButtonClicked;
        }

        private void OnStartButtonClicked()
        {
            _game.EnterGameplay();
        }

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
            
            _buttonStart.Focus();
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}
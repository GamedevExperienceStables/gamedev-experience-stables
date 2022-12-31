using Game.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class Hud : MonoBehaviour
    {
        private VisualElement _root;
        private Button _buttonMenu;
        private GameManager _game;

        public void Construct(GameManager game)
        {
            _game = game;
        }

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonMenu = _root.Q<Button>(LayoutNames.Hud.BUTTON_MENU);
            _buttonMenu.clicked += OnMenuClicked;
        }

        private void OnDestroy()
        {
            _buttonMenu.clicked -= OnMenuClicked;
        }

        private void OnMenuClicked()
        {
            _game.Pause();
        }

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}
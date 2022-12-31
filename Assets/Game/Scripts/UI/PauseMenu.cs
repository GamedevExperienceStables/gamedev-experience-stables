using Game.Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class PauseMenu : MonoBehaviour
    {
        private Button _buttonResume;
        private Button _buttonRestart;
        private Button _buttonExit;

        private VisualElement _root;
        private LevelManager _level;
        private GameManager _game;

        public void Construct(GameManager game, LevelManager level)
        {
            _game = game;
            _level = level;
        }

        public void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;

            _buttonResume = _root.Q<Button>(LayoutNames.PauseMenu.BUTTON_RESUME);
            _buttonRestart = _root.Q<Button>(LayoutNames.PauseMenu.BUTTON_RESTART);
            _buttonExit = _root.Q<Button>(LayoutNames.PauseMenu.BUTTON_EXIT);
      
            _buttonResume.clicked += ResumeGame;
            _buttonRestart.clicked += RestartLevel;
            _buttonExit.clicked += QuitGame;
        }

        public void OnDestroy()
        {
            _buttonResume.clicked -= ResumeGame;
            _buttonRestart.clicked -= RestartLevel;
            _buttonExit.clicked -= QuitGame;
        }

        private void RestartLevel()
        {
            _level.RestartLevel();
            _game.Resume();
        }

        private void QuitGame()
        {
            _game.QuitGame();
        }

        private void ResumeGame()
        {
            _game.Resume();
        }

        public void Show()
        {
            _root.style.display = DisplayStyle.Flex;
            
            _buttonResume.Focus();
        }

        public void Hide()
        {
            _root.style.display = DisplayStyle.None;
        }
    }
}
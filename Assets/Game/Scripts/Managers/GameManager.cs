using Game.Input;
using Game.UI;
using UnityEngine;

namespace Game.Managers
{
    public class GameManager : MonoBehaviour
    {
        private UIManager _ui;
        private LevelManager _level;
        private InputService _input;

        public void Construct(UIManager ui, LevelManager level, InputService input)
        {
            _ui = ui;
            _level = level;
            _input = input;
            
            _input.MenuButton.Performed += Pause;
            _input.CancelButton.Performed += Resume;
            _input.ExitButton.Performed += Resume;
        }
        
        public void OnDestroy()
        {
            _input.MenuButton.Performed -= Pause;
            _input.CancelButton.Performed -= Resume;
            _input.ExitButton.Performed -= Resume;
        }

        public void EnterMainMenu()
        {
            _ui.ShowStartMenu();
        }

        public void EnterGameplay()
        {
            _ui.ShowGameplay();
            _level.StartLevel();
        }
        
        public void Pause()
        {
            Time.timeScale = 0;

            _ui.ShowPauseMenu();
            _input.EnableMenus();
        }

        public void Resume()
        {
            Time.timeScale = 1;

            _ui.HidePauseMenu();
            _input.EnableGameplay();
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
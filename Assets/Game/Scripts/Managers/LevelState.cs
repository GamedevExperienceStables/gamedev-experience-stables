using Game.Input;
using Game.UI;
using UnityEngine;

namespace Game.Managers
{
    public class LevelState
    {
        private readonly InputService _input;
        private readonly LevelManager _level;
        private readonly UIManager _ui;

        public LevelState(InputService input, LevelManager level, UIManager ui)
        {
            _input = input;
            _level = level;
            _ui = ui;
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
        
    }
}
using Game.Managers;
using UnityEngine;

namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private StartMenu startMenu;

        [SerializeField]
        private Hud hud;

        [SerializeField]
        private PauseMenu pauseMenu;

        public void Construct(GameManager game, LevelManager level)
        {
            startMenu.Construct(game);
            pauseMenu.Construct(game, level);
            hud.Construct(game);
        }

        public void ShowStartMenu()
        {
            pauseMenu.Hide();
            hud.Hide();

            startMenu.Show();
        }

        public void ShowGameplay()
        {
            startMenu.Hide();
            pauseMenu.Hide();

            hud.Show();
        }

        public void ShowPauseMenu()
        {
            pauseMenu.Show();
        }

        public void HidePauseMenu()
        {
            pauseMenu.Hide();
        }
    }
}
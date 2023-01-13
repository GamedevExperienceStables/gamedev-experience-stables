using UnityEngine;

namespace Game.UI
{
    public class GameplayView : MonoBehaviour
    {
        [SerializeField]
        private HudView hudView;

        [SerializeField]
        private PauseMenuView pauseMenuView;

        public void Start()
        {
            hudView.Hide();
            pauseMenuView.Hide();
        }

        public void ShowHud()
        {
            hudView.Show();
            pauseMenuView.Hide();
        }

        public void ShowPause()
        {
            hudView.Hide();
            pauseMenuView.Show();
        }
    }
}
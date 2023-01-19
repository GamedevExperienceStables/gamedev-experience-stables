using Cysharp.Threading.Tasks;
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
            hudView.HideImmediate();
            pauseMenuView.HideImmediate();
        }

        public void ShowHud()
        {
            hudView.Show();
            pauseMenuView.Hide();
        }
        
        public UniTask ShowPauseAsync()
        {
            hudView.Hide();
            return pauseMenuView.ShowAsync();
        }

        public UniTask HidePauseAsync() 
            => pauseMenuView.HideAsync();
    }
}
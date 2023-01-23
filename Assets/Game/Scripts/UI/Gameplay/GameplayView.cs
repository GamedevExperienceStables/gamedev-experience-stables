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
            pauseMenuView.HideImmediate();
        }

        public UniTask ShowPauseAsync()
        {
            return UniTask.WhenAll(
                hudView.HideAsync(),
                pauseMenuView.ShowAsync()
            );
        }

        public UniTask HidePauseAsync()
        {
            return UniTask.WhenAll(
                hudView.ShowAsync(),
                pauseMenuView.HideAsync()
            );
        }
    }
}
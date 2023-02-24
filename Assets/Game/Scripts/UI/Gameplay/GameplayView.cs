using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class GameplayView : MonoBehaviour
    {
        private HudView _hudView;
        private PauseMenuView _pauseMenuView;
        private GameOverView _gameOverView;
        private InventoryView _inventoryView;

        private void Awake()
        {
            _hudView = GetComponentInChildren<HudView>();
            _pauseMenuView = GetComponentInChildren<PauseMenuView>();
            _gameOverView = GetComponentInChildren<GameOverView>();
            _inventoryView = GetComponentInChildren<InventoryView>();
        }

        public void Start()
        {
            _hudView.HideImmediate();
            _pauseMenuView.HideImmediate();
            _gameOverView.HideImmediate();
            _inventoryView.HideImmediate();
        }

        public void ShowHud()
        {
            _hudView.Show();
            _pauseMenuView.HideImmediate();
            _gameOverView.HideImmediate();
        }

        public UniTask ShowGameOverAsync()
        {
            return UniTask.WhenAll(
                _hudView.HideAsync(),
                _gameOverView.ShowAsync()
            );
        }

        public UniTask HideGameOverAsync()
            => _gameOverView.HideAsync();

        public UniTask ShowBookAsync()
            => _inventoryView.ShowAsync();

        public UniTask HideBookAsync()
            => _inventoryView.HideAsync();

        public UniTask ShowPauseAsync()
        {
            return UniTask.WhenAll(
                _hudView.HideAsync(),
                _pauseMenuView.ShowAsync()
            );
        }

        public UniTask HidePauseAsync()
        {
            return UniTask.WhenAll(
                _hudView.ShowAsync(),
                _pauseMenuView.HideAsync()
            );
        }
    }
}
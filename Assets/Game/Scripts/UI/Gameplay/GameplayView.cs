using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.UI
{
    public class GameplayView : MonoBehaviour
    {
        private HudView _hudView;
        private PauseView _pauseView;
        private GameOverView _gameOverView;
        private InventoryView _inventoryView;

        private void Awake()
        {
            _hudView = GetComponentInChildren<HudView>();
            _pauseView = GetComponentInChildren<PauseView>();
            _gameOverView = GetComponentInChildren<GameOverView>();
            _inventoryView = GetComponentInChildren<InventoryView>();
        }

        public void Start()
        {
            _hudView.HideImmediate();
            _pauseView.HideImmediate();
            _gameOverView.HideImmediate();
            _inventoryView.HideImmediate();
        }

        public void ShowHud()
        {
            _hudView.Show();
            _pauseView.HideImmediate();
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
                _pauseView.ShowAsync()
            );
        }

        public UniTask HidePauseAsync()
        {
            return UniTask.WhenAll(
                _hudView.ShowAsync(),
                _pauseView.HideAsync()
            );
        }
    }
}
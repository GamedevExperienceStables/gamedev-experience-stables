using System;
using Game.Input;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayMenuInput : IDisposable, IHistoryHandler<InputSchemeMenu>
    {
        private const int MAX_HISTORY_LENGTH = 6;
        
        private readonly IInputControlGameplay _inputGameplay;
        private readonly IInputControlMenu _inputMenu;

        private event Action BackRequested;
        private event Action InventoryRequested;
        private event Action MenuRequested;

        private readonly InputHistory<InputSchemeMenu> _history;

        [Inject]
        public GameplayMenuInput(IInputControlGameplay inputGameplay, IInputControlMenu inputMenu)
        {
            _inputGameplay = inputGameplay;
            _inputMenu = inputMenu;

            _history = new InputHistory<InputSchemeMenu>(MAX_HISTORY_LENGTH, InputSchemeMenu.None);

            SubscribeInputs();
        }

        public void Dispose()
            => UnSubscribeInputs();

        public void SubscribeBack(Action callback)
            => BackRequested += callback;

        public void UnSubscribeBack(Action callback)
            => BackRequested -= callback;

        public void SubscribeMenu(Action callback)
            => MenuRequested += callback;

        public void UnSubscribeMenu(Action callback)
            => MenuRequested -= callback;

        public void SubscribeInventory(Action callback)
            => InventoryRequested += callback;

        public void UnSubscribeInventory(Action callback)
            => InventoryRequested -= callback;

        public void PushState(InputSchemeMenu state)
            => _history.Push(state);

        public void ReplaceState(InputSchemeMenu scheme)
            => _history.Replace(scheme);

        public void Back(int depth = 1)
            => _history.Back(depth);

        private void SubscribeInputs()
        {
            _inputGameplay.MenuButton.Performed += OnMenu;
            _inputGameplay.InventoryButton.Performed += OnInventory;

            _inputMenu.BackButton.Performed += OnBack;
            _inputMenu.MenuButton.Performed += OnMenu;
            _inputMenu.OptionButton.Performed += OnInventory;
        }

        private void UnSubscribeInputs()
        {
            _inputGameplay.MenuButton.Performed -= OnMenu;
            _inputGameplay.InventoryButton.Performed -= OnInventory;

            _inputMenu.BackButton.Performed -= OnBack;
            _inputMenu.MenuButton.Performed -= OnMenu;
            _inputMenu.OptionButton.Performed -= OnInventory;
        }

        private void OnMenu()
        {
            switch (_history.Current)
            {
                case InputSchemeMenu.Gameplay:
                    MenuRequested?.Invoke();
                    break;

                case InputSchemeMenu.Pause:
                    BackRequested?.Invoke();
                    break;
            }
        }

        private void OnInventory()
        {
            switch (_history.Current)
            {
                case InputSchemeMenu.Gameplay:
                case InputSchemeMenu.Inventory:
                    InventoryRequested?.Invoke();
                    break;
            }
        }

        private void OnBack()
        {
            if (_history.Current is not InputSchemeMenu.Gameplay)
                BackRequested?.Invoke();
        }
    }
}
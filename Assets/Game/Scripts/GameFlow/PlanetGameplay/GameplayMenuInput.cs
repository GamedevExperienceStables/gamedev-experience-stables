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
        
        private event Action<int> ActiveSlotChanging;

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

        public void SubscribeActiveSlot(Action<int> callback)
            => ActiveSlotChanging += callback;
        
        public void UnSubscribeActiveSlot(Action<int> callback)
            => ActiveSlotChanging -= callback;

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
            
            _inputGameplay.Slot1Button.Performed += OnSlot1;
            _inputGameplay.Slot2Button.Performed += OnSlot2;
            _inputGameplay.Slot3Button.Performed += OnSlot3;
            _inputGameplay.Slot4Button.Performed += OnSlot4;
        }

        private void UnSubscribeInputs()
        {
            _inputGameplay.MenuButton.Performed -= OnMenu;
            _inputGameplay.InventoryButton.Performed -= OnInventory;

            _inputMenu.BackButton.Performed -= OnBack;
            _inputMenu.MenuButton.Performed -= OnMenu;
            _inputMenu.OptionButton.Performed -= OnInventory;
            
            _inputGameplay.Slot1Button.Performed -= OnSlot1;
            _inputGameplay.Slot2Button.Performed -= OnSlot2;
            _inputGameplay.Slot3Button.Performed -= OnSlot3;
            _inputGameplay.Slot4Button.Performed -= OnSlot4;
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
        
        private void OnSlot1() 
            => OnActiveSlotChanging(1);

        private void OnSlot2() 
            => OnActiveSlotChanging(2);

        private void OnSlot3() 
            => OnActiveSlotChanging(3);

        private void OnSlot4() 
            => OnActiveSlotChanging(4);

        private void OnActiveSlotChanging(int newSlotId) 
            => ActiveSlotChanging?.Invoke(newSlotId);
    }
}
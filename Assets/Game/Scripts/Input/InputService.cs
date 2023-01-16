using VContainer;

namespace Game.Input
{
    public class InputService : IInputService
    {
        private readonly GameInputControlsAdapter _gameInput;

        [Inject]
        public InputService(GameInputControlsAdapter gameInput)
        {
            _gameInput = gameInput;
        }

        public void EnableGameplay()
        {
            _gameInput.GameplayEnable();
            _gameInput.MenuDisable();
        }

        public void EnableMenus()
        {
            _gameInput.GameplayDisable();
            _gameInput.MenuEnable();
        }

        public void DisableAll()
        {
            _gameInput.GameplayDisable();
            _gameInput.MenuDisable();
        }
    }
}
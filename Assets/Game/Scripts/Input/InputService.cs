namespace Game.Input
{
    public partial class InputService : IInputReader
    {
        private readonly GameInputControls _gameInput;

        public InputService()
        {
            _gameInput = new GameInputControls();

            _gameInput.Gameplay.SetCallbacks(this);
            _gameInput.Menus.SetCallbacks(this);
        }

        public void EnableGameplay()
        {
            _gameInput.Gameplay.Enable();
            _gameInput.Menus.Disable();
        }

        public void EnableMenus()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Menus.Enable();
        }


        public void DisableAll()
        {
            _gameInput.Gameplay.Disable();
            _gameInput.Menus.Disable();
        }
    }
}
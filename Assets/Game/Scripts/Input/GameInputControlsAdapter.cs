using VContainer;

namespace Game.Input
{
    public class GameInputControlsAdapter
    {
        private readonly GameInputControls _controls;

        [Inject]
        public GameInputControlsAdapter()
            => _controls = new GameInputControls();

        public void SetGameplayCallbacks(GameInputControls.IGameplayActions callback)
            => _controls.Gameplay.SetCallbacks(callback);

        public void SetMenuCallbacks(GameInputControls.IMenuActions callback)
            => _controls.Menu.SetCallbacks(callback);

        public void GameplayEnable() => _controls.Gameplay.Enable();
        public void GameplayDisable() => _controls.Gameplay.Disable();
        public void MenuEnable() => _controls.Menu.Enable();
        public void MenuDisable() => _controls.Menu.Disable();
    }
}
using System;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Input
{
    public class InputBindings
    {
        private const int BINDING_MOUSE_KEYBOARD = 0;

        private readonly GameInputControls.GameplayActions _gameplayActions;
        private readonly GameInputControls.MenuActions _menuActions;

        [Inject]
        public InputBindings(GameInputControlsAdapter adapter)
        {
            _gameplayActions = adapter.GetGameplayActions();
            _menuActions = adapter.GetMenuActions();
        }

        public string GetBindingDisplayString(InputGameplayActions inputAction)
        {
            switch (inputAction)
            {
                case InputGameplayActions.Back:
                    return _menuActions.Back.GetBindingDisplayString(BINDING_MOUSE_KEYBOARD);

                case InputGameplayActions.Interaction:
                    return _gameplayActions.Interaction.GetBindingDisplayString(BINDING_MOUSE_KEYBOARD);

                default:
                    throw new ArgumentOutOfRangeException(nameof(inputAction), inputAction, null);
            }
        }
    }
}
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Input
{
    public class InputControlMenu : IInputControlMenu, GameInputControls.IMenuActions
    {
        public InputButton BackButton { get; } = new();
        public InputButton MenuButton { get; } = new();
        public InputButton OptionButton { get; } = new();

        [Inject]
        public InputControlMenu(GameInputControlsAdapter inputControls)
        {
            inputControls.SetMenuCallbacks(this);
        }

        public void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                BackButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                BackButton.TriggerButtonUp();
            }
        }

        public void OnMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MenuButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                MenuButton.TriggerButtonUp();
            }
        }

        public void OnOption(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OptionButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                OptionButton.TriggerButtonUp();
            }
        }
    }
}
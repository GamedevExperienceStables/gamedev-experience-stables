using UnityEngine.InputSystem;
using VContainer;

namespace Game.Input
{
    public class InputControlMenu : IInputControlMenu, GameInputControls.IMenuActions
    {
        public InputButton CancelButton { get; } = new();
        public InputButton ExitButton { get; } = new();

        [Inject]
        public InputControlMenu(GameInputControlsAdapter inputControls)
        {
            inputControls.SetMenuCallbacks(this);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CancelButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                CancelButton.TriggerButtonUp();
            }
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ExitButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                ExitButton.TriggerButtonUp();
            }
        }
    }
}
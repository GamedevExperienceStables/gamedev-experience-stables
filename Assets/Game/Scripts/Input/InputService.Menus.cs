using UnityEngine.InputSystem;

namespace Game.Input
{
    public partial class InputService : GameInputControls.IMenusActions
    {
        public InputButton CancelButton { get; } = new();
        public InputButton ExitButton { get; } = new();

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
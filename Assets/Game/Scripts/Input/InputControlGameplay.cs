using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Input
{
    public class InputControlGameplay : IInputControlGameplay, GameInputControls.IGameplayActions
    {
        public Vector2 PrimaryMovement { get; private set; }
        public Vector2 SecondaryMovement { get; private set; }
        public Vector2 MousePosition { get; private set; }

        public InputButton FireButton { get; } = new();
        public InputButton AimButton { get; } = new();
        public InputButton MenuButton { get; } = new();
        public InputButton InteractionButton { get; } = new();
        public InputButton DashButton { get; } = new();
        public InputButton Slot1Button { get; } = new();
        public InputButton Slot2Button { get; } = new();
        public InputButton Slot3Button { get; } = new();
        public InputButton Slot4Button { get; } = new();

        [Inject]
        public InputControlGameplay(GameInputControlsAdapter inputControls)
        {
            inputControls.SetGameplayCallbacks(this);
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AimButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                AimButton.TriggerButtonUp();
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                FireButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                FireButton.TriggerButtonUp();
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

        public void OnInteraction(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                InteractionButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                InteractionButton.TriggerButtonUp();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                DashButton.TriggerButtonDown();
            }
            else if (context.canceled)
            {
                DashButton.TriggerButtonUp();
            }
        }

        public void OnSlot1(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Slot1Button.TriggerButtonDown();
            }
        }

        public void OnSlot2(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Slot2Button.TriggerButtonDown();
            }
        }

        public void OnSlot3(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Slot3Button.TriggerButtonDown();
            }
        }

        public void OnSlot4(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Slot4Button.TriggerButtonDown();
            }
        }

        public void OnPrimaryMovement(InputAction.CallbackContext context)
        {
            PrimaryMovement = context.ReadValue<Vector2>();
        }

        public void OnSecondaryMovement(InputAction.CallbackContext context)
        {
            SecondaryMovement = context.ReadValue<Vector2>();
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }
    }
}
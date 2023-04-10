using UnityEngine;

namespace Game.Input
{
    public interface IInputControlGameplay
    {
        Vector2 PrimaryMovement { get; }
        Vector2 SecondaryMovement { get; }
        Vector2 MousePosition { get; }
        InputButton FireButton { get; }
        InputButton MeleeButton { get; }
        InputButton AimButton { get; }
        InputButton MenuButton { get; }
        InputButton InventoryButton { get; }
        InputButton InteractionButton { get; }
        InputButton DashButton { get; }
        InputButton Slot1Button { get; }
        InputButton Slot2Button { get; }
        InputButton Slot3Button { get; }
        InputButton Slot4Button { get; }
    }
}
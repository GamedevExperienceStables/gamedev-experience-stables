using UnityEngine;

namespace Game.Input
{
    public interface IInputReader
    {
        Vector2 PrimaryMovement { get; }
        Vector2 SecondaryMovement { get; }
        Vector2 MousePosition { get; }
        InputButton FireButton { get; }
        InputButton AimButton { get; }
        InputButton MenuButton { get; }
        
        InputButton CancelButton { get; }
        InputButton ExitButton { get; }
    }
}
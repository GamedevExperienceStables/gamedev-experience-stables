using System;

namespace Game.Actors
{
    [Flags]
    public enum InputBlock
    {
        Movement = 1,
        Rotation = 2,
        Action = 4
    }

    public static class InputBlockExtensions
    {
        public const InputBlock FULL_BLOCK = InputBlock.Action | InputBlock.Movement | InputBlock.Rotation;

        public static bool HasFlagFast(this InputBlock value, InputBlock flag) 
            => (value & flag) != 0;
    }
}
using System;

namespace Game.Actors
{
    [Flags]
    public enum InputBlock
    {
        Movement = 1,
        Rotation = 1 << 1,
        Action = 1 << 2,
        Interact = 1 << 3
    }

    public static class InputBlockExtensions
    {
        public const InputBlock FULL_BLOCK = (InputBlock)~0;

        public static bool HasFlagFast(this InputBlock value, InputBlock flag)
            => (value & flag) != 0;
    }
}
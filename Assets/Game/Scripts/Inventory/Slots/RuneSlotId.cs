using System;

namespace Game.Inventory
{
    public readonly struct RuneSlotId : IEquatable<RuneSlotId>
    {
        private readonly int _id;

        private RuneSlotId(int id)
            => _id = id;

        public bool Equals(RuneSlotId other)
            => _id == other._id;

        public override bool Equals(object obj)
            => obj is RuneSlotId other && Equals(other);

        public override int GetHashCode()
            => _id;

        public static bool operator ==(RuneSlotId left, RuneSlotId right)
            => left.Equals(right);

        public static bool operator !=(RuneSlotId left, RuneSlotId right)
            => !left.Equals(right);

        public override string ToString()
            => _id.ToString();

        public static implicit operator RuneSlotId(int id)
            => new(id);

        public static explicit operator int(RuneSlotId slotId)
            => slotId._id;
    }
}
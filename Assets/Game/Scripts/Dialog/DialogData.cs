using System;
using Game.Level;

namespace Game.Dialog
{
    public readonly struct DialogData : IEquatable<DialogData>
    {
        public DialogData(IDialogDefinition definition, bool oneShot = false)
        {
            Definition = definition;
            OneShot = oneShot;
        }

        public IDialogDefinition Definition { get; }

        public bool OneShot { get; }

        public bool Equals(DialogData other)
            => Equals(Definition, other.Definition);

        public override bool Equals(object obj)
            => obj is DialogData other && Equals(other);

        public override int GetHashCode()
            => Definition != null ? Definition.GetHashCode() : 0;

        public static bool operator ==(DialogData left, DialogData right)
            => left.Equals(right);

        public static bool operator !=(DialogData left, DialogData right)
            => !left.Equals(right);
    }
}
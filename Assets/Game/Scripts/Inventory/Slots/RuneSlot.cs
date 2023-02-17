namespace Game.Inventory
{
    public class RuneSlot
    {
        private readonly RuneSlotId _id;

        public RuneSlot(RuneSlotId slotId)
            => _id = slotId;

        public bool IsEmpty => ReferenceEquals(Rune, null);
        public RuneDefinition Rune { get; private set; }

        public void Set(RuneDefinition rune)
            => Rune = rune;

        public void Clear()
            => Rune = null;
    }
}
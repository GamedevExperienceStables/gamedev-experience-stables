namespace Game.Inventory
{
    public class RuneSlot
    {
        public RuneSlot(RuneSlotId slotId)
            => Id = slotId;

        public RuneSlotId Id { get; }
        public RuneDefinition Rune { get; private set; }
        
        public bool IsEmpty => ReferenceEquals(Rune, null);

        public void Set(RuneDefinition rune)
            => Rune = rune;

        public void Clear()
            => Rune = null;
    }
}
namespace Game.Inventory
{
    public class RuneActiveSlot
    {
        public RuneSlotId Value { get; private set; } = RuneSlotId.INVALID_ID;

        public bool IsEmpty => !Value.IsValid();

        public void Set(RuneSlotId id)
            => Value = id;

        public void Clear()
            => Value = RuneSlotId.INVALID_ID;
    }
}
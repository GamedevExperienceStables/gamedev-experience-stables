using Game.Inventory;

namespace Game.UI
{
    public struct RuneSlotRemoveEvent
    {
        public int pointerId;
        public RuneSlotId id;
        public RuneDefinition definition;
    }
}
using Game.Inventory;

namespace Game.UI
{
    public struct RuneSlotHoverEvent
    {
        public int pointerId;
        public RuneDefinition definition;
        public bool state;
    }
}
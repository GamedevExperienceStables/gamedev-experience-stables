using Game.Inventory;

namespace Game.UI
{
    public struct RuneSlotDragCompleteEvent
    {
        public RuneSlotHudView source;
        public RuneSlotHudView target;
        
        public RuneDefinition rune;
    }
}
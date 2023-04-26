using Game.Settings;
using VContainer;

namespace Game.Inventory
{
    public class InventoryData
    {
        [Inject]
        public InventoryData(InventorySettings settings, LevelsSettings levelsSettings)
        {
            Materials = new Materials(levelsSettings.Levels, settings.BagMaxStack);
            Runes = new Runes();
            Slots = new RuneSlots(settings.InventorySlots);
        }

        public Materials Materials { get; }
        public Runes Runes { get; }
        public RuneSlots Slots { get; }
    }
}
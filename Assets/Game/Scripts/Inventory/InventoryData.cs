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
            Recipes = new Recipes();
            Runes = new Runes();
            Slots = new RuneSlots(settings.InventorySlots);
        }

        public Materials Materials { get; }
        public Recipes Recipes { get; }
        public Runes Runes { get; }
        public RuneSlots Slots { get; }
    }
}
using Game.Inventory;
using Game.Settings;
using VContainer;

namespace Game.Level
{
    public class LevelGoalChecker
    {
        private readonly LevelController _level;
        private readonly IInventoryItems _inventory;

        [Inject]
        public LevelGoalChecker(LevelController level, IInventoryItems inventory)
        {
            _level = level;
            _inventory = inventory;
        }

        public bool AreGoalsMet()
        {
            LevelDefinition definition = _level.GetCurrentLevel();
            int currentMaterialCount = _inventory.Materials.Container.GetCurrentValue(definition.Goal.Material);
            if (currentMaterialCount < definition.Goal.Count)
                return false;

            // check if waves are destroyed
            
            return true;
        }
    }
}
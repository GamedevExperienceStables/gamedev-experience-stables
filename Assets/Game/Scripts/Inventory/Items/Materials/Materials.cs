using System.Collections.Generic;
using Game.Settings;

namespace Game.Inventory
{
    public class Materials
    {
        private Dictionary<MaterialDefinition, MaterialData> _items = new();

        public void Init(LevelsSettings settings)
        {
            _items.Clear();
            foreach (LevelDefinition level in settings.Levels)
            {
                LevelGoalSettings goal = level.Goal;
                _items[goal.Material] = new MaterialData(goal.Material, goal.Count, 0);
            }
        }
    }
}
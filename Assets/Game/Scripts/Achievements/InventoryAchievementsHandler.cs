using System;
using Game.Inventory;
using VContainer;
using VContainer.Unity;

namespace Game.Achievements
{
    public sealed class InventoryAchievementsHandler : IStartable, IDisposable
    {
        private readonly IAchievementsService _achievements;
        private readonly RuneAchievementsSettings _settings;
        private readonly IInventoryItems _items;

        [Inject]
        public InventoryAchievementsHandler(IAchievementsService achievements, RuneAchievementsSettings settings,
            IInventoryItems items)
        {
            _achievements = achievements;
            _settings = settings;
            _items = items;
        }

        public void Start()
            => _items.AddedToBag += OnItemAddedToBag;

        public void Dispose()
            => _items.AddedToBag -= OnItemAddedToBag;

        private void OnItemAddedToBag(ItemDefinition item)
        {
            if (item is RuneDefinition rune)
                AddedAchievement(rune);
        }

        private void AddedAchievement(RuneDefinition rune)
        {
            if (_settings.GetKey(rune, out string key))
                _achievements.TryAddAchievement(key);
        }
    }
}
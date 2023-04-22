using VContainer;

namespace Game.Level
{
    public class LocationStateStore
    {
        private readonly LocationStateStoreLoot _loot;
        private readonly LocationStateStoreCounters _counters;

        [Inject]
        public LocationStateStore(LocationStateStoreLoot loot, LocationStateStoreCounters counters)
        {
            _loot = loot;
            _counters = counters;
        }

        public void Init(LocationData locationData, ILocationContext context)
        {
            _counters.Restore(locationData.Counters, context);
            _loot.Restore(locationData.Loot);
        }

        public void Store(LocationData locationData, ILocationContext context)
        {
            _counters.Store(locationData.Counters, context);
            _loot.Store(locationData.Loot, context);
        }
    }
}
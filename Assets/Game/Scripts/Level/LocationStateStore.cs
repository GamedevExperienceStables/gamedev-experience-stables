using VContainer;

namespace Game.Level
{
    public class LocationStateStore
    {
        private readonly LocationStateStoreLoot _loot;
        private readonly LocationStateStoreCounter _counters;
        private readonly LocationStateStoreFact _facts;

        [Inject]
        public LocationStateStore(LocationStateStoreLoot loot, LocationStateStoreCounter counters,
            LocationStateStoreFact facts)
        {
            _loot = loot;
            _counters = counters;
            _facts = facts;
        }

        public void Init(LocationData locationData, ILocationContext context)
        {
            _counters.Restore(locationData.Counters, context);
            _facts.Restore(locationData.Facts, context);
            _loot.Restore(locationData.Loot);
        }

        public void Store(LocationData locationData, ILocationContext context)
        {
            _counters.Store(locationData.Counters, context);
            _facts.Store(locationData.Facts, context);
            _loot.Store(locationData.Loot, context);
        }
    }
}
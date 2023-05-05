using VContainer;

namespace Game.Level
{
    public class LocationStateStore
    {
        private readonly LocationStateStoreLoot _loot;
        private readonly LocationStateStoreCounter _counters;
        private readonly LocationStateStoreFact _facts;
        private readonly LevelController _level;
        private readonly ILocationContext _context;

        [Inject]
        public LocationStateStore(LocationStateStoreLoot loot, LocationStateStoreCounter counters,
            LocationStateStoreFact facts, LevelController level, ILocationContext context)
        {
            _loot = loot;
            _counters = counters;
            _facts = facts;
            _level = level;
            _context = context;
        }

        public void Init()
        {
            LocationData locationData = GetLocationData();
            
            _counters.Restore(locationData.Counters, _context);
            _facts.Restore(locationData.Facts, _context);
            _loot.Restore(locationData.Loot);
        }

        public void Store()
        {
            LocationData locationData = GetLocationData();

            _counters.Store(locationData.Counters, _context);
            _facts.Store(locationData.Facts, _context);
            _loot.Store(locationData.Loot, _context);
        }

        private LocationData GetLocationData()
        {
            ILocationDefinition locationDefinition = _context.Location;
            LocationData locationData = _level.GetOrCreateLocationData(locationDefinition);
            return locationData;
        }
    }
}
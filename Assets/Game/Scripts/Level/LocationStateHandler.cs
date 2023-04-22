using VContainer;

namespace Game.Level
{
    public class LocationStateHandler
    {
        private readonly LevelController _level;
        private readonly LocationStateHandlerLoot _loot;
        private readonly LocationStateHandlerCounters _counters;

        private LocationData _locationData;

        [Inject]
        public LocationStateHandler(
            LevelController level,
            LocationStateHandlerLoot loot,
            LocationStateHandlerCounters counters
        )
        {
            _level = level;
            _loot = loot;
            _counters = counters;
        }

        public void Init(ILocationDefinition definition, ILocationContext context)
        {
            _locationData = _level.GetOrCreateLocationData(definition);

            _counters.Restore(_locationData.Counters, context);
            _loot.Restore(_locationData.Loot);
        }

        public void Store(ILocationContext context)
        {
            _counters.Store(_locationData.Counters, context);
            _loot.Store(_locationData.Loot, context);

            _locationData = null;
        }
    }
}
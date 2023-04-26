using VContainer;

namespace Game.Level
{
    public class LocationDataHandler
    {
        private readonly LocationData _locationData;

        [Inject]
        public LocationDataHandler(LocationData locationData)
        {
            _locationData = locationData;
        }

        public bool CanTransferTo(LocationPointDefinition targetLocation)
            => _locationData.allowExit;
    }
}
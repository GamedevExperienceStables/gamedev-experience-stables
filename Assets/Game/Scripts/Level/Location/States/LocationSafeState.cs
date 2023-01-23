using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class LocationSafeState : GameState
    {
        private readonly LocationData _locationData;

        [Inject]
        public LocationSafeState(LocationData locationData) 
            => _locationData = locationData;

        protected override void OnEnter()
        {
            _locationData.allowExit = true;
            _locationData.allowInventory = true;
        }

        protected override void OnExit()
        {
        }
    }
}
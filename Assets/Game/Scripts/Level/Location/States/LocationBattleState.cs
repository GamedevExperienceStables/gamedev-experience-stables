using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class LocationBattleState : GameState
    {
        private readonly LocationData _locationData;

        [Inject]
        public LocationBattleState(LocationData locationData) 
            => _locationData = locationData;
        
        protected override void OnEnter()
        {
            _locationData.allowExit = false;
            _locationData.allowInventory = false;
        }

        protected override void OnExit()
        {
            // hide ui
        }
    }
}
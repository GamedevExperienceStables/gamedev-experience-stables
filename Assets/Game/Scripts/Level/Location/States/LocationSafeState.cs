using Cysharp.Threading.Tasks;
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

        protected override UniTask OnEnter()
        {
            _locationData.allowExit = true;
            _locationData.allowInventory = true;
            
            return UniTask.CompletedTask;
        }
    }
}
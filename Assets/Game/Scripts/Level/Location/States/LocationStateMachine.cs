using Game.GameFlow;
using VContainer;

namespace Game.Level
{
    public class LocationStateMachine : GameStateMachine
    {
        [Inject]
        public LocationStateMachine(
            LocationBattleState battleState, 
            LocationSafeState safeState
            )
        {
            stateMachine.AddState(safeState);
            stateMachine.AddState(battleState);
        }
    }
}
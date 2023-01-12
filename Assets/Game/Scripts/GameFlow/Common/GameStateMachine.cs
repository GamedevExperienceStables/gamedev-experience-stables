using Game.Utils;

namespace Game.GameFlow
{
    public abstract class GameStateMachine 
    {
        protected readonly StateMachine stateMachine = new();
        
        public void EnterState<T>() where T : class, IState 
            => stateMachine.EnterState<T>();
    }
}
using Game.Utils;

namespace Game.GameFlow
{
    public abstract class GameStateMachine
    {
        protected readonly StateMachine stateMachine = new();

        public void EnterState<T>() where T : class, IState
            => stateMachine.EnterState<T>();
        
        public void PushState<T>() where T : class, IState 
            => stateMachine.PushState<T>();

        public void PopState() 
            => stateMachine.PopState();
    }
}
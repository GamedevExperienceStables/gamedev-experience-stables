using Cysharp.Threading.Tasks;
using Game.Utils;

namespace Game.GameFlow
{
    public abstract class GameStateMachine
    {
        protected readonly StateMachine stateMachine = new();

        public void EnterState<T>() where T : class, IState
            => stateMachine.EnterState<T>().Forget();
        
        public void PushState<T>() where T : class, IState 
            => stateMachine.PushState<T>().Forget();

        public void PopState() 
            => stateMachine.PopState().Forget();
    }
}
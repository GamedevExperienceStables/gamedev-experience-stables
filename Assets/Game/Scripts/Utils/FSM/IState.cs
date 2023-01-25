namespace Game.Utils
{
    public interface IState
    {
        StateMachine Parent { get; set; }
        StateMachine Child { get; set; }

        void Enter();
        void Exit();

    }
}
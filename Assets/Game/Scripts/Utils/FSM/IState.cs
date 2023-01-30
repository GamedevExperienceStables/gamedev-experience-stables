using Cysharp.Threading.Tasks;

namespace Game.Utils
{
    public interface IState
    {
        StateMachine Parent { get; set; }
        StateMachine Child { get; set; }

        UniTask Enter();
        UniTask Exit();
    }
}
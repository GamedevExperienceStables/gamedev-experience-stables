using Cysharp.Threading.Tasks;
using Game.Utils;
using UnityEngine;

namespace Game.GameFlow
{
    public abstract class GameState : IState
    {
        public StateMachine Parent { get; set; }
        public StateMachine Child { get; set; }

        public UniTask Enter()
        {
            Debug.Log($"[STATE] <color=cyan>>>></color> {this}");
            
            return OnEnter();
        }

        public UniTask Exit()
        {
            Child?.Exit();
            
            Debug.Log($"[STATE] <color=grey><<<</color> {this}");
            
            return OnExit();
        }

        protected abstract UniTask OnEnter();

        protected virtual UniTask OnExit() 
            => UniTask.CompletedTask;
    }
}
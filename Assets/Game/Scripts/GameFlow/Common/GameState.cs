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
#if UNITY_EDITOR
            Debug.Log($"[STATE] <color=cyan>>>></color> {this}");
#endif
            return OnEnter();
        }

        public UniTask Exit()
        {
            Child?.Exit();
#if UNITY_EDITOR            
            Debug.Log($"[STATE] <color=grey><<<</color> {this}");
#endif            
            return OnExit();
        }

        protected abstract UniTask OnEnter();

        protected virtual UniTask OnExit() 
            => UniTask.CompletedTask;
    }
}
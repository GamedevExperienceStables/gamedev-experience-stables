using Game.Utils;
using UnityEngine;

namespace Game.GameFlow
{
    public abstract class GameState : IState
    {
        public StateMachine Parent { get; set; }
        public StateMachine Child { get; set; }

        public virtual void Enter()
        {
            Debug.Log($"[STATE] <color=cyan>>>></color> {this}");

            OnEnter();
        }

        public virtual void Exit()
        {
            Child?.CurrentState.Exit();

            Debug.Log($"[STATE] <color=grey><<<</color> {this}");

            OnExit();
        }

        protected abstract void OnEnter();

        protected abstract void OnExit();
    }
}
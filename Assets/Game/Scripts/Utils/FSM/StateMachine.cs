using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Utils
{
    public class StateMachine
    {
        private readonly Stack<IState> _stack = new();
        private readonly Dictionary<Type, IState> _states = new();

        private IState _currentState;

        public void AddState<T>(T state) where T : class, IState
        {
            Type type = typeof(T);
            if (_states.ContainsKey(type))
            {
                Debug.LogWarning($"Trying add state '{type}' that already registered.");
                return;
            }

            _states[type] = state;
            state.Parent = this;
        }

        public async UniTask EnterState<T>() where T : class, IState
        {
            Type type = typeof(T);
            if (_currentState?.GetType() == type)
            {
                Debug.LogWarning($"Trying to enter state '{type}' that already entered.");
                return;
            }

            _currentState = await ChangeState<T>();
            await _currentState.Enter();
        }
        
        public async UniTask PushState<T>() where T : class, IState
        {
            var state = GetState<T>();
            _stack.Push(state);
            
            await state.Enter();
        }
        
        public async UniTask PopState()
        {
            if (_stack.Count == 0)
            {
                Debug.LogWarning("Trying to pop state when stack is empty.");
                return;
            }

            IState state = _stack.Pop();
            await state.Exit();
        }

        private async UniTask<T> ChangeState<T>() where T : class, IState
        {
            if (_currentState is not null)
                await _currentState.Exit();

            return GetState<T>();
        }

        private T GetState<T>() where T : class, IState
            => _states[typeof(T)] as T;
        
        
        public async UniTask Exit()
        {
            while(_stack.Count > 0)
                await PopState();
            
            await _currentState.Exit();
        }
    }
}
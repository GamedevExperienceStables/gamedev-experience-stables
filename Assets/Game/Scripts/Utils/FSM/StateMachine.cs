using System;
using System.Collections.Generic;
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

        public void EnterState<T>() where T : class, IState
        {
            Type type = typeof(T);
            if (_currentState?.GetType() == type)
            {
                Debug.LogWarning($"Trying to enter state '{type}' that already entered.");
                return;
            }

            _currentState = ChangeState<T>();
            _currentState.Enter();
        }
        
        public void PushState<T>() where T : class, IState
        {
            var state = GetState<T>();
            _stack.Push(state);
            
            state.Enter();
        }
        
        public void PopState()
        {
            if (_stack.Count == 0)
            {
                Debug.LogWarning("Trying to pop state when stack is empty.");
                return;
            }

            _stack.Pop().Exit();
        }

        private T ChangeState<T>() where T : class, IState
        {
            _currentState?.Exit();

            var state = GetState<T>();
            _currentState = state;

            return state;
        }

        private T GetState<T>() where T : class, IState
            => _states[typeof(T)] as T;
        
        
        public void Exit()
        {
            while(_stack.Count > 0)
                PopState();
            
            _currentState.Exit();
        }
    }
}
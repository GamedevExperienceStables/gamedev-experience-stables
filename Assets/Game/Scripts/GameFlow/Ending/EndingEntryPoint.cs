using System;
using Game.Input;
using VContainer;
using VContainer.Unity;

namespace Game.GameFlow.Ending
{
    public sealed class EndingEntryPoint : IStartable, IDisposable
    {
        private readonly IInputService _input;
        private readonly IInputControlMenu _menuControl;
        private readonly RootStateMachine _rootStateMachine;

        [Inject]
        public EndingEntryPoint(IInputService input, IInputControlMenu menuControl, RootStateMachine rootStateMachine)
        {
            _input = input;
            _menuControl = menuControl;
            _rootStateMachine = rootStateMachine;
        }

        public void Start()
        {
            _input.ReplaceState(InputSchemeGame.Menu);
            
            _menuControl.BackButton.Performed += OnExitButton;
        }

        public void Dispose() 
            => _menuControl.BackButton.Performed -= OnExitButton;

        private void OnExitButton()
            => _rootStateMachine.EnterState<MainMenuState>();
    }
}
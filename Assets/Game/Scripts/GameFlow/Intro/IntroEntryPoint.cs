using Game.Input;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class IntroEntryPoint : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;
        private IInputService _input;
        private IInputControlMenu _menuControl;

        [Inject]
        public void Construct(RootStateMachine rootStateMachine, IInputService input, IInputControlMenu menuControl)
        {
            _input = input;
            _menuControl = menuControl;
            _rootStateMachine = rootStateMachine;
        }

        private void Start()
        {
            _input.ReplaceState(InputSchemeGame.Menu);
            _menuControl.BackButton.Performed += OnExitButton;
        }

        private void OnDestroy()
            => _menuControl.BackButton.Performed -= OnExitButton;

        private void OnExitButton()
            => Complete();


        public void Complete()
            => _rootStateMachine.EnterState<PlanetState>();
    }
}
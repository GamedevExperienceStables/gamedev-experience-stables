using Game.Input;
using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.GameFlow
{
    [RequireComponent(typeof(UIDocument))]
    public class IntroView : MonoBehaviour
    {
        [SerializeField]
        private CutsceneDefinition cutscene;

        private CutsceneView _cutscene;

        private RootStateMachine _rootStateMachine;
        private IInputService _input;
        private IInputControlMenu _menuControl;

        [Inject]
        public void Construct(CutsceneView view, RootStateMachine rootStateMachine, IInputService input,
            IInputControlMenu menuControl)
        {
            _cutscene = view;

            _rootStateMachine = rootStateMachine;
            _input = input;
            _menuControl = menuControl;
        }

        private void Start()
        {
            var document = GetComponent<UIDocument>();

            _cutscene.Create(document.rootVisualElement);
            _cutscene.Start(cutscene.Slides);
            _cutscene.Completed += OnCompleted;

            _menuControl.BackButton.Performed += OnExitButton;
            _menuControl.BackButton.Canceled += OnExitButtonRelease;

            _menuControl.ConfirmButton.Performed += OnConfirm;

            _input.ReplaceState(InputSchemeGame.Menu);
        }

        private void OnDestroy()
        {
            _cutscene.Completed -= OnCompleted;

            _menuControl.BackButton.Performed -= OnExitButton;
            _menuControl.BackButton.Canceled -= OnExitButtonRelease;

            _menuControl.ConfirmButton.Performed -= OnConfirm;
            
            _cutscene.Destroy();
        }

        private void OnExitButton()
            => _cutscene.StartHoldExit();

        private void OnExitButtonRelease()
            => _cutscene.StopHoldExit();

        private void OnCompleted()
            => Complete();

        private void Complete()
            => _rootStateMachine.EnterState<PlanetState>();

        private void OnConfirm()
            => _cutscene.Next();
    }
}
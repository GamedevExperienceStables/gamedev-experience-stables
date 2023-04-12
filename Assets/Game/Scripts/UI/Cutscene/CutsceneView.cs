using System;
using Game.Input;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class CutsceneView : MonoBehaviour
    {
        [SerializeField]
        private CutsceneDefinition cutscene;

        private Cutscene _cutscene;

        private IInputService _input;
        private IInputControlMenu _menuControl;
        
        public event Action Completed;

        [Inject]
        public void Construct(Cutscene view, IInputService input, IInputControlMenu menuControl)
        {
            _cutscene = view;

            _input = input;
            _menuControl = menuControl;
        }

        public void Play()
        {
            _input.ReplaceState(InputSchemeGame.Menu);
            _cutscene.Start(cutscene.Slides);
        }

        private void Awake()
        {
            var document = GetComponent<UIDocument>();

            _cutscene.Create(document.rootVisualElement, destroyCancellationToken);
            _cutscene.Completed += OnCompleted;

            _menuControl.BackButton.Performed += OnExitButton;
            _menuControl.BackButton.Canceled += OnExitButtonRelease;

            _menuControl.ConfirmButton.Performed += OnConfirm;
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
            => Completed?.Invoke();

        private void OnConfirm()
            => _cutscene.Next();
    }
}
using Game.UI;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class CreditsScreen : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;

        [SerializeField]
        private CreditsView credits;

        [Inject]
        public void Construct(RootStateMachine rootStateMachine) 
            => _rootStateMachine = rootStateMachine;

        private void Start() 
            => credits.Play(OnCompleted);

        private void OnCompleted()
            => _rootStateMachine.EnterState<MainMenuState>();
    }
}
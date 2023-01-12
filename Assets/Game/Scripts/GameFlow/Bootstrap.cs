using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class Bootstrap : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;

        [Inject]
        private void Construct(RootStateMachine rootStateMachine)
        {
            _rootStateMachine = rootStateMachine;
        }
        
        private void Start()
        {
            _rootStateMachine.EnterState<InitState>();
        }
    }
}
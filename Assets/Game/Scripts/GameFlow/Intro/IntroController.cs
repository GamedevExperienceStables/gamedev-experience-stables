using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class IntroController : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;

        [Inject]
        public void Construct(RootStateMachine rootStateMachine)
            => _rootStateMachine = rootStateMachine;


        public void Complete()
            => _rootStateMachine.EnterState<PlanetState>();
    }
}
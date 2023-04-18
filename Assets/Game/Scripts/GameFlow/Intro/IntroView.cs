using Game.UI;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    [RequireComponent(typeof(CutsceneView))]
    public class IntroView : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;
        private CutsceneView _cutscene;

        [Inject]
        public void Construct(RootStateMachine rootStateMachine)
            => _rootStateMachine = rootStateMachine;

        private void Awake()
        {
            _cutscene = GetComponent<CutsceneView>();
            _cutscene.Completed += OnCompleted;
        }

        private void Start()
            => _cutscene.Play();

        private void OnCompleted()
            => _rootStateMachine.EnterState<PlanetState>();
    }
}
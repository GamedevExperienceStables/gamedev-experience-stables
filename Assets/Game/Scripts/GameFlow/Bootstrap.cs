using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.GameFlow
{
    public class Bootstrap : MonoBehaviour
    {
        private RootStateMachine _rootStateMachine;

        [SerializeField, Range(0f, 10f)]
        private float waitTime = 3f;

        [Inject]
        private void Construct(RootStateMachine rootStateMachine)
        {
            _rootStateMachine = rootStateMachine;
        }

        private void Start()
            => Boot().Forget();

        private async UniTaskVoid Boot()
        {
            await AwaitSplashScreen();

            _rootStateMachine.EnterState<InitState>();
        }

        private async UniTask AwaitSplashScreen()
            => await UniTask.Delay(TimeSpan.FromSeconds(waitTime));
    }
}
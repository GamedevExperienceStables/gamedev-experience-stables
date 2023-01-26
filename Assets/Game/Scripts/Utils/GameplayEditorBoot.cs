using Game.GameFlow;
using Game.Level;
using Game.Persistence;
using UnityEngine;
using VContainer;

namespace Game.Utils
{
    public class GameplayEditorBoot : MonoBehaviour
    {
        [SerializeField]
        private LocationPointDefinition currentLocation;
        
        private GameDataHandler _game;
        private RootStateMachine _rootStateMachine;
        private LevelDataHandler _level;

        [Inject]
        public void Construct(GameDataHandler game, LevelDataHandler level, RootStateMachine rootStateMachine)
        {
            _game = game;
            _level = level;
            _rootStateMachine = rootStateMachine;
        }

        public void Start()
        {
            _game.Reset();
            _level.SetLocation(currentLocation);

            _rootStateMachine.EnterState<PlanetState>();
        }
    }
}
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
        
        private GameImportExport _game;
        private RootStateMachine _rootStateMachine;
        private LevelController _level;

        [Inject]
        public void Construct(GameImportExport game, LevelController level, RootStateMachine rootStateMachine)
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
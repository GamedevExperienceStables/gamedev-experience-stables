using Game.Settings;
using VContainer;

namespace Game.GameFlow
{
    public class NewGameState : GameState
    {
        private readonly GameStateModel _gameStateModel;
        private readonly InitialSettings _initialSettings;

        [Inject]
        public NewGameState(
            GameStateModel gameStateModel,
            InitialSettings initialSettings
        )
        {
            _gameStateModel = gameStateModel;
            _initialSettings = initialSettings;
        }


        protected override void OnEnter()
        {
            NewGame();
            
            Parent.EnterState<IntroState>();
        }

        private void NewGame()
        {
            _gameStateModel.CurrentLocation = _initialSettings.StartPoint;
        }

        protected override void OnExit()
        {
        }
    }
}
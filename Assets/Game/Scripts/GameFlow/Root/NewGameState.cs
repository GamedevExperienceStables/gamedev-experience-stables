using Cysharp.Threading.Tasks;
using Game.Persistence;
using VContainer;

namespace Game.GameFlow
{
    public class NewGameState : GameState
    {
        private readonly PersistenceService _persistenceService;

        [Inject]
        public NewGameState(PersistenceService persistenceService) 
            => _persistenceService = persistenceService;

        protected override async UniTask OnEnter()
        {
            _persistenceService.InitData();
            await _persistenceService.SaveDataAsync();

            await Parent.EnterState<IntroState>();
        }
    }
}
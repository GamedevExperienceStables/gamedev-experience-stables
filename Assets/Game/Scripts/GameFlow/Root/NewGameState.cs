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

        protected override UniTask OnEnter()
        {
            _persistenceService.InitData();

            return Parent.EnterState<IntroState>();
        }
    }
}
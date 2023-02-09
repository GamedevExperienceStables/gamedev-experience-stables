using Cysharp.Threading.Tasks;
using Game.Persistence;
using VContainer;

namespace Game.Level
{
    public class SaveGameInteraction : Interaction
    {
        private readonly PersistenceService _persistence;

        [Inject]
        public SaveGameInteraction(PersistenceService persistence)
            => _persistence = persistence;

        public override bool CanExecute()
            => !_persistence.IsRunning;

        public override void Execute()
            => _persistence.SaveDataAsync().Forget();
    }
}
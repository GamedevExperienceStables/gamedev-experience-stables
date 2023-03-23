using System;
using Game.Persistence;
using VContainer;

namespace Game.UI
{
    public class SavingViewModel
    {
        private readonly PersistenceService _persistence;

        [Inject]
        public SavingViewModel(PersistenceService persistence)
            => _persistence = persistence;

        public void Subscribe(Action<bool> callback)
            => _persistence.SavingChanged += callback;

        public void UnSubscribe(Action<bool> callback)
            => _persistence.SavingChanged -= callback;
    }
}
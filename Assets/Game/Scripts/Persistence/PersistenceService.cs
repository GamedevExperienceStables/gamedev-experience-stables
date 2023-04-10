using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Game.Persistence
{
    public class PersistenceService
    {
        private readonly IPersistence _persistence;

        private readonly Settings _settings;
        private readonly GameImportExport _game;
        
        public event Action<bool> SavingChanged;

        [Inject]
        public PersistenceService(Settings settings, GameImportExport game, IPersistence persistence)
        {
            _settings = settings;
            _game = game;

            _persistence = persistence;
        }

        public bool IsRunning { get; private set; }

        public bool IsSaveGameExists()
            => _persistence.Exists(_settings.Filename);

        public void InitData()
            => _game.Reset();

        public async UniTask LoadDataAsync()
        {
            if (IsRunning)
            {
                Debug.LogWarning("Loading in process.");
                return;
            }

            IsRunning = true;
            await LoadingProcess();
            IsRunning = false;
        }

        public async UniTask SaveDataAsync()
        {
            if (IsRunning)
            {
                Debug.LogWarning("Saving in process.");
                return;
            }

            IsRunning = true;
            await SavingProcess();
            IsRunning = false;
        }

        private async UniTask SavingProcess()
        {
            SavingChanged?.Invoke(true);
            
            GameSaveData data = _game.Export();
            await _persistence.SerializeAsync(data, _settings.Filename);
            
            SavingChanged?.Invoke(false);
#if UNITY_EDITOR
            Debug.Log("[SAVE_GAME] Saved!");
#endif
        }

        private async UniTask LoadingProcess()
        {
            var data = await _persistence.DeserializeAsync<GameSaveData>(_settings.Filename);
            _game.Import(data);
#if UNITY_EDITOR
            Debug.Log("[SAVE_GAME] Loaded!");
#endif
        }

        [Serializable]
        public class Settings
        {
            [SerializeField]
            private string filename = "data.sav";

            [SerializeField]
            private bool formatting;

            public string Filename => filename;
            public bool Formatting => formatting;
        }
    }
}
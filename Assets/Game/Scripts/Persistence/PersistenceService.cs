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
        private readonly GameDataHandler _handler;
        
        [Inject]
        public PersistenceService(Settings settings, GameDataHandler handler, IPersistence persistence)
        {
            _settings = settings;
            _handler = handler;

            _persistence = persistence;
        }

        public void InitData() 
            => _handler.Reset();

        public async UniTask LoadDataAsync()
        {
            var data = await _persistence.DeserializeAsync<GameSaveData>(_settings.Filename);
            _handler.Import(data);
        }

        public UniTask SaveDataAsync()
        {
            GameSaveData data = _handler.Export();
            return _persistence.SerializeAsync(data, _settings.Filename);
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
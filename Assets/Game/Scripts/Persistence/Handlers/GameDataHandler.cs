using System;
using System.Globalization;
using Game.GameFlow;
using VContainer;

namespace Game.Persistence
{
    public class GameDataHandler
    {
        private readonly GameData _gameData;
        private readonly LevelDataHandler _levelDataHandler;
        private readonly PlayerDataHandler _playerDataHandler;

        [Inject]
        public GameDataHandler(
            GameData gameData,
            LevelDataHandler levelDataHandler,
            PlayerDataHandler playerDataHandler
        )
        {
            _gameData = gameData;
            _levelDataHandler = levelDataHandler;
            _playerDataHandler = playerDataHandler;
        }

        public void Reset()
        {
            _gameData.PlayTime = TimeSpan.Zero;
            _gameData.SessionStartTime = DateTime.Now;

            _levelDataHandler.Reset();
            _playerDataHandler.Reset();
        }

        public GameSaveData Export()
        {
            var data = new GameSaveData
            {
                meta = new GameSaveData.MetaSaveData
                {
                    timestampString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    playTime = _gameData.PlayTime.Milliseconds,
                },
                level = _levelDataHandler.Export(),
                player = _playerDataHandler.Export()
            };

            return data;
        }

        public void Import(GameSaveData data)
        {
            _gameData.SessionStartTime = DateTime.Now;
            _gameData.PlayTime = TimeSpan.FromMilliseconds(data.meta.playTime);

            _levelDataHandler.Import(data.level);
            _playerDataHandler.Import(data.player);
        }
    }
}
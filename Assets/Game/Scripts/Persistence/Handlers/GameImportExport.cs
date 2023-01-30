using System;
using System.Globalization;
using Game.GameFlow;
using VContainer;

namespace Game.Persistence
{
    public class GameImportExport
    {
        private readonly GameData _game;
        private readonly LevelImportExport _level;
        private readonly PlayerImportExport _player;

        [Inject]
        public GameImportExport(
            GameData game,
            LevelImportExport level,
            PlayerImportExport player
        )
        {
            _game = game;
            _level = level;
            _player = player;
        }

        public void Reset()
        {
            _game.PlayTime = TimeSpan.Zero;
            _game.SessionStartTime = DateTime.Now;

            _level.Reset();
            _player.Reset();
        }

        public GameSaveData Export()
        {
            var data = new GameSaveData
            {
                meta = new GameSaveData.MetaSaveData
                {
                    timestampString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    playTime = _game.PlayTime.Milliseconds,
                },
                level = _level.Export(),
                player = _player.Export()
            };

            return data;
        }

        public void Import(GameSaveData data)
        {
            _game.SessionStartTime = DateTime.Now;
            _game.PlayTime = TimeSpan.FromMilliseconds(data.meta.playTime);

            _level.Import(data.level);
            _player.Import(data.player);
        }
    }
}
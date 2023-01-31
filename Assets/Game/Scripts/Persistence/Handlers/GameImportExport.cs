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
            TimeSpan totalPlayTime = CalculateTotalPlayTime();
            var data = new GameSaveData
            {
                meta = new GameSaveData.Meta
                {
                    timestampString = DateTime.Now.ToString(CultureInfo.InvariantCulture),
                    playTime = (uint)totalPlayTime.TotalSeconds
                },
                level = _level.Export(),
                player = _player.Export()
            };

            return data;
        }

        private TimeSpan CalculateTotalPlayTime()
        {
            TimeSpan lastPlayTime = _game.PlayTime;
            TimeSpan currentSessionPlayTime = DateTime.Now.Subtract(_game.SessionStartTime);
            TimeSpan totalPlayTime = currentSessionPlayTime + lastPlayTime;
            return totalPlayTime;
        }

        public void Import(GameSaveData data)
        {
            _game.SessionStartTime = DateTime.Now;
            _game.PlayTime = TimeSpan.FromSeconds(data.meta.playTime);

            _level.Import(data.level);
            _player.Import(data.player);
        }
    }
}
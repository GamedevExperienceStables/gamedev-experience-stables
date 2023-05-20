using System;
using Game.TimeManagement;
using Steamworks;
using VContainer;
using VContainer.Unity;

namespace Game.Steam
{
    public sealed class SteamOverlay : IStartable, IDisposable
    {
        private readonly SteamService _steam;
        private readonly ITimeService _timeService;

        private Callback<GameOverlayActivated_t> _gameOverlayActivated;

        [Inject]
        public SteamOverlay(SteamService steam, ITimeService timeService)
        {
            _steam = steam;
            _timeService = timeService;
        }

        public void Start()
            => _gameOverlayActivated = _steam.CreateCallback<GameOverlayActivated_t>(OnGameOverlayActivated);

        public void Dispose()
            => _gameOverlayActivated.Dispose();

        private void OnGameOverlayActivated(GameOverlayActivated_t param)
        {
            if (param.m_bActive != 0)
                _timeService.Pause();
            else
                _timeService.Play();
        }
    }
}
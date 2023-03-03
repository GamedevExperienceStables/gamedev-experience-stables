using Game.Level;
using Game.Player;
using UnityEngine;
using VContainer;

namespace Game.UI
{
    public class MiniMapViewModel
    {
        private readonly PlayerController _player;
        private readonly LocationController _location;

        [Inject]
        public MiniMapViewModel(PlayerController player, LocationController location)
        {
            _player = player;
            _location = location;
        }

        public Vector3 HeroPosition
            => _player.GetHeroPosition();

        public Quaternion HeroRotation
            => _player.GetHeroRotation();

        public Vector3 MapCenter
            => _location.GetLevelBoundary()?.Center ?? Vector3.zero;

        public Vector3 MapSize
            => _location.GetLevelBoundary()?.Size ?? Vector3.zero;
    }
}
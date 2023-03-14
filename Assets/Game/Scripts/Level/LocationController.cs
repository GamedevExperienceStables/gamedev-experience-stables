using Game.Audio;
using Game.Cameras;
using Game.Hero;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class LocationController
    {
        private readonly HeroFactory _heroFactory;

        private readonly FollowSceneCamera _followCamera;
        private readonly LocationAudioListener _audioListener;
        private readonly LocationStateMachine _locationStateMachine;

        private HeroController _hero;
        private LocationContext _context;

        [Inject]
        public LocationController(
            HeroFactory heroFactory,
            FollowSceneCamera followCamera,
            LocationAudioListener audioListener,
            LocationStateMachine locationStateMachine
        )
        {
            _heroFactory = heroFactory;
            _followCamera = followCamera;
            _audioListener = audioListener;
            _locationStateMachine = locationStateMachine;
        }

        public void Init(LocationContext locationContext, Transform spawnPoint)
        {
            _context = locationContext;

            SpawnHero(spawnPoint);

            _context.InitEnemySpawners(_hero.transform);

            _locationStateMachine.EnterState<LocationSafeState>();
        }

        public void Clear()
        {
            if (_hero)
            {
                _hero.Reset();
                _hero.SetActive(false);
            }

            if (_context)
                _context = null;
        }

        private void SpawnHero(Transform spawnPoint)
        {
            if (!_hero) 
                _hero = _heroFactory.Create();

            UnbindHero();
            
            _hero.SetActive(true);
            _hero.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            _followCamera.SetTarget(_hero.CameraTarget);
            _audioListener.SetTarget(_hero.CameraTarget);
        }

        public void UnbindHero()
        {
            _followCamera.ClearTarget();
            _audioListener.ClearTarget();
        }

        public ILevelBoundary GetLevelBoundary()
        {
            return _context.FindBoundaries();
        }
    }
}
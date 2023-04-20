using System;
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

        private HeroController _hero;
        private LocationContext _context;

        public event Action Initialized;

        [Inject]
        public LocationController(
            HeroFactory heroFactory,
            FollowSceneCamera followCamera,
            LocationAudioListener audioListener,
        )
        {
            _heroFactory = heroFactory;
            _followCamera = followCamera;
            _audioListener = audioListener;
        }

        public Bounds Bounds { get; private set; }
        public ILocationDefinition LocationDefinition { get; private set; }

        public Transform Hero => _hero.transform;

        public void Init(ILocationDefinition definition, LocationContext locationContext, Transform spawnPoint)
        {
            LocationDefinition = definition;
            _context = locationContext;

            SpawnHero(spawnPoint);
            InitLocationBounds();

            _context.InitEnemySpawners(_hero.transform);

            Initialized?.Invoke();
        }

        private void InitLocationBounds()
        {
            ILocationBounds bounds = _context.FindBounds();
            if (bounds is null) 
                return;
            
            Bounds = new Bounds(bounds.Center, bounds.Size);
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

            UnFollowHero();
            
            _hero.SetActive(true);
            _hero.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            
            FollowHero();
        }

        private void FollowHero()
        {
            _followCamera.SetTarget(_hero.CameraTarget);
            _audioListener.SetTarget(_hero.CameraTarget);
        }

        public void UnFollowHero()
        {
            _followCamera.ClearTarget();
            _audioListener.ClearTarget();
        }
    }
}
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
        private readonly LevelController _level;

        private HeroController _hero;
        private LocationContext _context;

        public event Action Initialized;

        [Inject]
        public LocationController(
            HeroFactory heroFactory,
            FollowSceneCamera followCamera,
            LocationAudioListener audioListener,
            LevelController level
        )
        {
            _heroFactory = heroFactory;
            _followCamera = followCamera;
            _audioListener = audioListener;
            _level = level;
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

            InitLocationState();

            Initialized?.Invoke();
        }

        public void Clear()
        {
            if (_hero)
            {
                _hero.Reset();
                _hero.SetActive(false);
            }

            if (_context)
            {
                PreserveLocationState();

                _context = null;
            }
        }

        private void InitLocationState()
            => InitCountersState();

        private void InitCountersState()
        {
            LocationCounters counters = _level.GetLocationCounters(LocationDefinition);
            foreach (ILocationCounter locationSpawn in _context.FindCounters())
            {
                if (counters.TryGetCounter(locationSpawn.Id, out LocationCounterData data))
                    locationSpawn.SetCount(data.count);
            }
        }

        private void PreserveLocationState()
            => PreserveCountersState();

        private void PreserveCountersState()
        {
            LocationCounters counters = _level.GetLocationCounters(LocationDefinition);
            foreach (ILocationCounter counter in _context.FindCounters())
            {
                if (counter.IsDirty)
                    counters.SetCounter(counter.Id, counter.RemainingCount);
            }
        }

        private void InitLocationBounds()
        {
            ILocationBounds bounds = _context.FindBounds();
            if (bounds is null)
                return;

            Bounds = new Bounds(bounds.Center, bounds.Size);
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
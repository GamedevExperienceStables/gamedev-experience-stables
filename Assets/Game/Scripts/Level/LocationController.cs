using System;
using Game.Audio;
using Game.Cameras;
using Game.Enemies;
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

        public event Action Initialized;

        [Inject]
        public LocationController(
            HeroFactory heroFactory,
            FollowSceneCamera followCamera,
            LocationAudioListener audioListener
        )
        {
            _heroFactory = heroFactory;
            _followCamera = followCamera;
            _audioListener = audioListener;
        }
        
        public Transform Hero => _hero.transform;

        public void Init(ILocationContext context, ILocationPointKey locationPoint)
        {
            LocationPoint spawnPoint = context.FindLocationPoint(locationPoint);
            SpawnHero(spawnPoint.transform);
            
            InitEnemySpawners(context, _hero.transform);

            Initialized?.Invoke();
        }

        public void Clear()
        {
            if (_hero)
            {
                _hero.Reset();
                _hero.SetActive(false);
            }
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
        
        private static void InitEnemySpawners(ILocationContext context, Transform target)
        {
            var enemySpawnZones = context.FindAll<EnemySpawnGroup>();
            if (enemySpawnZones.Count == 0)
                return;

            Transform spawnContainer = CreateContainer();
            foreach (EnemySpawnGroup spawnZone in enemySpawnZones)
                spawnZone.Init(spawnContainer, target);
        }
        
        private static Transform CreateContainer() 
            => new GameObject("EnemiesContainer").transform;
    }
}
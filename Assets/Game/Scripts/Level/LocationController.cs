using Game.Cameras;
using Game.Hero;
using UnityEngine;
using VContainer;

namespace Game.Level
{
    public class LocationController
    {
        private readonly HeroFactory _heroFactory;

        private readonly SceneCamera _sceneCamera;
        private readonly FollowSceneCamera _followCamera;

        private HeroController _hero;
        private LocationContext _context;

        [Inject]
        public LocationController(
            HeroFactory heroFactory,
            SceneCamera sceneCamera,
            FollowSceneCamera followCamera
        )
        {
            _heroFactory = heroFactory;
            _sceneCamera = sceneCamera;
            _followCamera = followCamera;
        }

        public void Init(LocationContext locationContext, Transform spawnPoint)
        {
            _context = locationContext;

            _hero = SpawnHero(spawnPoint);
            _followCamera.SetTarget(_hero.CameraTarget);
            _context.InitEnemySpawners(_hero.transform);
        }

        public void Clear()
        {
            if (_context)
            {
                DestroyEnemies();
                _context = null;
            }

            if (_hero)
                DestroyHero();
        }

        private HeroController SpawnHero(Transform spawnPoint)
            => _heroFactory.Create(spawnPoint, _sceneCamera, _followCamera);

        private void DestroyHero()
        {
            _followCamera.ClearTarget();
            Object.Destroy(_hero.gameObject);

            _hero = null;
        }

        private void DestroyEnemies()
        {
            _context.DestroyEnemies();
        }
    }
}
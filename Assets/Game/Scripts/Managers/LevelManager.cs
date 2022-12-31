using Game.Cameras;
using Game.Hero;
using Game.Input;
using UnityEngine;

namespace Game.Managers
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private SceneCamera sceneCamera;

        [SerializeField]
        private FollowSceneCamera followCamera;
        
        [Space]
        [SerializeField]
        private LevelContext level;

        [Header("Prefabs")]
        [SerializeField]
        private HeroController heroPrefab;

        private InputService _input;
        private HeroController _hero;

        public void Construct(InputService inputService)
        {
            _input = inputService;
        }

        public void StartLevel()
        {
            _input.EnableGameplay();

            _hero = SpawnHero();
            followCamera.SetTarget(_hero.CameraTarget);
            level.InitEnemySpawners(_hero.transform);
        }

        public void RestartLevel()
        {
            level.DestroyEnemies();
            DestroyHero();

            StartLevel();
        }

        private HeroController SpawnHero()
        {
            HeroController hero = Instantiate(heroPrefab);
            hero.Construct(_input, sceneCamera, followCamera);
            
            level.MoveToSpawnPoint(hero);

            return hero;
        }

        private void DestroyHero()
        {
            if (_hero)
            {
                followCamera.ClearTarget();
                Destroy(_hero.gameObject);

                _hero = null;
            }
        }
    }
}
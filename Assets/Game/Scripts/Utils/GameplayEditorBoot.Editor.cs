#if UNITY_EDITOR
using System.Data;
using Game.GameFlow;
using Game.Level;
using Game.Persistence;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Game.Utils
{
    public partial class GameplayEditorBoot
    {
        private const float GIZMO_CAMERA_POINT_RADIUS = 0.5f;

        public enum SpawnTargetMethod
        {
            CameraPosition,
            SelectedSpawnPoint,
        }

        [Header("Spawn Settings")]
        [SerializeField]
        private SpawnTargetMethod spawnMethod;

        [SerializeField, Required("Select spawn point on scene")]
        [ShowIf(nameof(spawnMethod), SpawnTargetMethod.SelectedSpawnPoint)]
        private LocationPoint selectedSpawnPoint;

        private GameImportExport _game;
        private RootStateMachine _stateMachine;
        private LevelController _level;

        private static GameplayEditorBoot _firstInstance;
        private static LocationPointKey _fakePointKey;
        private static bool _isFinalized;

        protected override void Awake()
        {
            base.Awake();

            if (!_firstInstance)
                BootInit();
            else if (!_isFinalized)
                BootFinalize();
            else
                Destroy(gameObject);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_firstInstance != this)
                return;

            _firstInstance = null;
            _fakePointKey = null;
            _isFinalized = false;
        }

        private void BootInit()
        {
            _firstInstance = this;

            DontDestroyOnLoad(this);

            ResolveDependencies();
            ResetGameData();
            DestroyCamera();
            SetLocationPoint();

            _stateMachine.EnterState<PlanetState>();
        }

        private void BootFinalize()
        {
            _isFinalized = true;

            if (spawnMethod == SpawnTargetMethod.CameraPosition)
                CreateSpawnPointLocation();

            Destroy(gameObject);
        }

        private void CreateSpawnPointLocation()
        {
            LifetimeScope locationScope = FindOrCreateLifetimeScope();
            
            var spawnPoint = new GameObject("CameraHeroSpawnPoint");
            var locationPoint = spawnPoint.AddComponent<LocationPoint>();
            locationPoint.SetPointKey(_fakePointKey);

            Vector3 spawnPosition = GetSpawnPositionFromCamera();
            spawnPoint.transform.position = spawnPosition;

            spawnPoint.transform.SetParent(locationScope.transform);
            spawnPoint.transform.SetAsFirstSibling();
        }

        private static LifetimeScope FindOrCreateLifetimeScope()
        {
            LifetimeScope locationScope = Find<LocationLifetimeScope>();
            
            // ReSharper disable once InvertIf
            if (!locationScope)
            {
                var locationContext = new GameObject("LocationContext");
                locationContext.AddComponent<LocationContext>();
                locationScope = locationContext.AddComponent<LocationLifetimeScope>();
            }

            return locationScope;
        }

        private void ResetGameData()
            => _game.Reset();

        private void ResolveDependencies()
        {
            _game = Container.Resolve<GameImportExport>();
            _stateMachine = Container.Resolve<RootStateMachine>();
            _level = Container.Resolve<LevelController>();
        }

        private void SetLocationPoint()
        {
            ILocationPointKeyOwner locationPoint;
            if (spawnMethod == SpawnTargetMethod.SelectedSpawnPoint)
            {
                if (!selectedSpawnPoint)
                    throw new NoNullAllowedException($"{gameObject.name} has not selected spawn point!");

                locationPoint = CreateLocationPoint(selectedSpawnPoint.PointKey);
            }
            else
            {
                _fakePointKey = CreateLocationPointKey();
                locationPoint = CreateLocationPoint(_fakePointKey);
            }

            _level.SetLocationPoint(locationPoint);
        }

        private static LocationPointKey CreateLocationPointKey()
            => ScriptableObject.CreateInstance<LocationPointKey>();

        private static ILocationPoint CreateLocationPoint(ILocationPointKey locationPointKey)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            var location = new Location(activeScene.name);
            var locationPoint = new LocationPointData(location, locationPointKey);

            return locationPoint;
        }

        private void DestroyCamera()
        {
            var editorCamera = GetComponentInChildren<Camera>();
            Destroy(editorCamera.gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Vector3 spawnPoint = GetSpawnPositionFromCamera();
            if (spawnPoint == Vector3.zero)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnPoint, GIZMO_CAMERA_POINT_RADIUS);
        }

        private Vector3 GetSpawnPositionFromCamera()
        {
            var editorCamera = GetComponentInChildren<Camera>();
            if (!editorCamera)
                return Vector3.zero;

            Transform editorCameraTransform = editorCamera.transform;
            Vector3 position = editorCameraTransform.position;
            Vector3 height = editorCameraTransform.localPosition;
            Vector3 lookDirection = editorCameraTransform.forward;

            var cameraRay = new Ray(position, lookDirection);
            var plane = new Plane(Vector3.up, position - height);
            if (!plane.Raycast(cameraRay, out float distance))
                return Vector3.zero;

            if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, distance))
                return hitInfo.point;

            return cameraRay.GetPoint(distance);
        }
    }
}
#endif
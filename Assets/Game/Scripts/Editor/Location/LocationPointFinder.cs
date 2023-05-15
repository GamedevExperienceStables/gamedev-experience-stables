using Game.Level;
using Game.UI;
using Game.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Editor.Location
{
    public class LocationPointFinder : EditorWindow
    {
        private const string MENU_PATH = "Tools/Editor/Level Map Point Finder";

        private Vector2Field _coordinatesField;

        [MenuItem(MENU_PATH, true, 10)]
        public static bool Validate()
            => !EditorApplication.isPlaying;

        [MenuItem(MENU_PATH, false, 10)]
        public static void Init()
        {
            var window = GetWindow<LocationPointFinder>("Location Point Finder");
            window.Show();
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            _coordinatesField = new Vector2Field("Coordinates");
            root.Add(_coordinatesField);

            var button = new Button(OnMoveClicked)
            {
                text = "Move To"
            };

            root.Add(button);
        }

        private void OnMoveClicked()
            => FindPointOnScene();

        private void FindPointOnScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();

            if (!TryGetLocationMap(activeScene.name, out Sprite map))
            {
                Debug.LogError("Could not find Location Map, aborting");
                return;
            }

            if (!FindLocationBounds(activeScene, out LocationBounds locationBounds))
            {
                Debug.LogError("Could not find Location Bounds, aborting");
                return;
            }

            Bounds worldBounds = locationBounds.GetComponent<BoxCollider>().bounds;
            Vector2 mapPosition = _coordinatesField.value;

            MoveCameraToPosition(map, worldBounds, mapPosition);
        }

        private void MoveCameraToPosition(Sprite map, Bounds worldBounds, Vector2 mapPosition)
        {
            Vector3 targetPosition = GetWorldPosition(worldBounds, map, mapPosition);
            // targetPosition = _target.transform.TransformPoint(targetPosition);

            var sceneView = SceneView.lastActiveSceneView;
            sceneView.LookAt(targetPosition);
            sceneView.Repaint();

            DebugExtensions.DebugWireSphere(targetPosition, Color.red, duration: 0.5f);
        }


        private static bool TryGetLocationMap(string sceneName, out Sprite map)
        {
            foreach (string guid in AssetDatabase.FindAssets($"t:{nameof(LocationDefinition)}"))
            {
                var item = AssetDatabase.LoadAssetAtPath<LocationDefinition>(AssetDatabase.GUIDToAssetPath(guid));
                if (item.SceneName != sceneName)
                    continue;

                map = item.MapImage;
                return true;
            }

            map = null;
            return false;
        }

        private static bool FindLocationBounds(Scene activeScene, out LocationBounds bounds)
        {
            foreach (GameObject rootGameObject in activeScene.GetRootGameObjects())
            {
                bounds = rootGameObject.GetComponentInChildren<LocationBounds>();
                if (bounds is not null)
                    return true;
            }

            bounds = default;
            return false;
        }

        private Vector3 GetWorldPosition(Bounds worldBounds, Sprite map, Vector2 coordinates)
        {
            MiniMapExtensions.GetMapData(map, out Bounds mapBounds, out _);
            Vector3 targetPosition = MiniMapExtensions.MapToWorldPosition(coordinates, worldBounds, mapBounds);
            return targetPosition;
        }
    }
}
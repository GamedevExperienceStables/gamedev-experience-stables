using System.IO;
using System.Threading.Tasks;
using Game.Level;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Game.Editor.Tools
{
    public static class MapCreator
    {
        private const string MENU_PATH = "Tools/Editor/Create Level Map";

        private static readonly string MapDirectory = Path.Combine("Game", "Art", "Locations");


        [MenuItem(MENU_PATH, true, 10)]
        public static bool ValidateX4() => Validate();

        [MenuItem(MENU_PATH, false, 10)]
        public static void CreateMapX4() => CreateScreenshot(4);

        private static bool Validate()
            => !EditorApplication.isPlaying;

        private static async Task CreateScreenshot(int size)
        {
            Scene activeScene = SceneManager.GetActiveScene();
            if (!FindLocationBounds(activeScene, out LocationBounds bounds))
                Debug.LogWarning("Could not find Location Bounds");

            var boxCollider = bounds.GetComponent<BoxCollider>();
            Camera camera = CreateCamera(boxCollider);

            bool cacheRenderFog = RenderSettings.fog;
            RenderSettings.fog = false;

            try
            {
                await Task.Delay(10);
                await TakeScreenshot(camera, boxCollider.size, activeScene.name, size);
            }
            finally
            {
                RenderSettings.fog = cacheRenderFog;
                Object.DestroyImmediate(camera.gameObject);
            }
        }

        private static Camera CreateCamera(BoxCollider boxCollider)
        {
            var go = new GameObject("ScreenShotCamera");
            var camera = go.AddComponent<Camera>();
            go.AddComponent<UniversalAdditionalCameraData>();

            Transform transform = camera.transform;
            Vector3 center = boxCollider.transform.position + boxCollider.center;
            transform.position = center + Vector3.up;
            transform.LookAt(center);

            camera.orthographic = true;
            camera.orthographicSize = boxCollider.size.z * 0.5f;

            return camera;
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


        private static async Task TakeScreenshot(Camera camera, Vector3 boundsSize, string sceneName, int size)
        {
            int width = (int)boundsSize.x * size;
            int height = (int)boundsSize.z * size;

            RenderTexture renderTexture = CreateRenderTexture(camera, width, height);
            Texture2D screenshot = CreateTexture(width, height);
            Clear(camera, renderTexture);

            string filename = await CreateFile(sceneName, width, height, screenshot);
            Debug.Log($"Map saved to {filename}");
        }

        private static async Task<string> CreateFile(string sceneName, int width, int height, Texture2D screenshot)
        {
            string filename = CreateFileName(width, height, sceneName);
            await File.WriteAllBytesAsync(filename, screenshot.EncodeToPNG());
            AssetDatabase.Refresh();
            return filename;
        }

        private static void Clear(Camera camera, RenderTexture renderTexture)
        {
            camera.targetTexture = null;
            RenderTexture.active = null;
            Object.DestroyImmediate(renderTexture);
        }

        private static Texture2D CreateTexture(int width, int height)
        {
            var screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            return screenshot;
        }

        private static RenderTexture CreateRenderTexture(Camera camera, int width, int height)
        {
            var renderTexture = new RenderTexture(width, height, 24);
            camera.targetTexture = renderTexture;
            camera.Render();

            RenderTexture.active = renderTexture;
            return renderTexture;
        }

        private static string CreateFileName(int width, int height, string sceneName)
        {
            string directory = Path.Combine(Application.dataPath, MapDirectory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string fileName = $"{sceneName}_{width}x{height}.png";
            return Path.Combine(directory, fileName);
        }
    }
}
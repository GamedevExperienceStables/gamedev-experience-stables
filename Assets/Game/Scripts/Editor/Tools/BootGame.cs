using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Editor.Tools
{
    public static class BootGame
    {
        private const string MENU_PATH = "Game/Play %#&p";
        private const string LAST_SCENE_KEY = "Editor/LastScene";
        private static string BootScenePath => EditorBuildSettings.scenes[0].path;

        private static string LastScene
        {
            get => EditorPrefs.GetString(LAST_SCENE_KEY);
            set => EditorPrefs.SetString(LAST_SCENE_KEY, value);
        }

        static BootGame()
            => EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        [MenuItem(MENU_PATH, true)]
        public static bool Validate()
            => !EditorApplication.isPlaying;

        [MenuItem(MENU_PATH)]
        public static void StartGame()
            => LoadBootScene();

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                OnEnteredEditMode();
        }

        private static void LoadBootScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            string bootScenePath = BootScenePath;
            if (string.IsNullOrEmpty(bootScenePath))
            {
                Debug.LogError("Boot scene is not set");
                return;
            }

            if (!IsSceneContainsInBuild(bootScenePath))
            {
                Debug.LogError("Boot scene is not in build");
                return;
            }

            Scene activeScene = SceneManager.GetActiveScene();
            if (string.IsNullOrEmpty(activeScene.path))
            {
                Debug.LogError("Active scene is not set");
                return;
            }

            if (bootScenePath != activeScene.path) 
                LastScene = activeScene.path;

            EditorSceneManager.OpenScene(bootScenePath);
            EditorApplication.isPlaying = true;
        }

        private static bool IsSceneContainsInBuild(string bootScenePath)
            => System.Array.Exists(EditorBuildSettings.scenes, scene => scene.path == bootScenePath);

        private static void OnEnteredEditMode()
        {
            string lastScene = LastScene;
            if (string.IsNullOrEmpty(lastScene))
                return;

            LastScene = string.Empty;
            EditorSceneManager.OpenScene(lastScene);
        }
    }
}
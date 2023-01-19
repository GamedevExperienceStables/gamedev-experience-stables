using System;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Game.SceneManagement
{
    public class SceneLoader
    {
        private readonly ILoadingScreen _loadingScreen;

        [Inject]
        public SceneLoader(ILoadingScreen loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public async UniTask<Scene> LoadSceneAsync(string sceneName, Action onLoaded = null)
        {
            await _loadingScreen.ShowAsync();

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                if (operation.progress >= 0.9f)
                    operation.allowSceneActivation = true;

                await UniTask.Yield();
            }

            onLoaded?.Invoke();

            _loadingScreen.Hide();
            return SceneManager.GetSceneByName(sceneName);
        }

        public async UniTask<Scene> LoadSingleSceneAdditiveAsync(string sceneName)
        {
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(scene);

            return scene;
        }

        public async UniTask UnloadSceneIfLoadedAsync(string sceneName)
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
                return;

            await SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
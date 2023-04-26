using VContainer;

namespace Game.GameFlow
{
    public class QuitGameService
    {
        [Inject]
        public QuitGameService()
        {
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}
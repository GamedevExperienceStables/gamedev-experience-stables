using Cysharp.Threading.Tasks;

namespace Game.UI
{
    public interface ILoadingScreen
    {
        void Show();
        void Hide();
        
        UniTask ShowAsync();
        UniTask HideAsync();
    }
}
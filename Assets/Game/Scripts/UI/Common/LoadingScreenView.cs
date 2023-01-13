using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class LoadingScreenView : MonoBehaviour, ILoadingScreen
    {
        private VisualElement _container;
        
        private void Awake()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            
            _container = root.Q<VisualElement>("loading-screen");
        }

        public void Show()
        {
            _container.style.display = DisplayStyle.Flex;
        }

        public void Hide()
        {
            _container.style.display = DisplayStyle.None;
        }
    }
}
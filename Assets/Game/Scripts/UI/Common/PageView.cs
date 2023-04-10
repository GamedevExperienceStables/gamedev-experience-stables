using Game.Localization;
using Game.Utils;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class PageView : MonoBehaviour
    {
        private VisualElement _root;
        protected VisualElement Content { get; private set; }
        
        protected ILocalizationService _localization;
        
        [Inject]
        public void Construct(ILocalizationService localisation) 
            => _localization = localisation;

        private void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            Content = _root;

            OnAwake();
        }

        protected void SetContent(string elementId)
            => Content = _root.Q<VisualElement>(elementId);

        public virtual void Show()
            => Content.SetDisplay(true);

        public virtual void Hide()
            => Content.SetDisplay(false);

        protected abstract void OnAwake();
    }


    public abstract class PageView<T> : PageView
    {
        protected T ViewModel { get; private set; }

        [Inject]
        public void Construct(T viewModel)
            => ViewModel = viewModel;
    }
}
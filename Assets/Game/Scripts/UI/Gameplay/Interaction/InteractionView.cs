using Game.Level;
using Game.Utils;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public sealed class InteractionView
    {
        private readonly InteractionViewModel _viewModel;
        private VisualElement _container;
        
        private Label _label;

        [Inject]
        public InteractionView(InteractionViewModel viewModel)
        {
            _viewModel = viewModel;

            _viewModel.SubscribeEnabled(OnEnabled);
            _viewModel.SubscribeDisabled(OnDisabled);
        }

        public void Destroy()
        {
            _viewModel.UnSubscribeEnabled(OnEnabled);
            _viewModel.UnSubscribeDisabled(OnDisabled);
        }

        public void Create(VisualElement root)
        {
            _container = root.Q<VisualElement>(LayoutNames.Hud.WIDGET_INTERACTION);
            _container.SetDisplay(false);

            _label = _container.Q<Label>(LayoutNames.Hud.WIDGET_INTERACTION_TEXT);
        }

        private void OnEnabled(Interaction newInteraction) 
            => Show();

        private void OnDisabled() 
            => Hide();

        private void Show() 
            => _container.SetDisplay(true);

        private void Hide() 
            => _container.SetDisplay(false);
    }
}
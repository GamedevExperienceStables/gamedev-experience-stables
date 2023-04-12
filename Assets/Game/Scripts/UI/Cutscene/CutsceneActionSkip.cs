using Game.Input;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class CutsceneActionSkip
    {
        private readonly InputBindings _bindings;
        private readonly Cutscene.Settings _settings;
        private VisualElement _container;

        public CutsceneActionSkip(InputBindings bindings, Cutscene.Settings settings)
        {
            _bindings = bindings;
            _settings = settings;
        }

        public void Create(VisualElement root)
        {
            _container = root.Q<VisualElement>(LayoutNames.Cutscene.BLOCK_SKIP);

            string button = _bindings.GetBindingDisplayString(InputGameplayActions.Back);
            root.Q<Label>(LayoutNames.Cutscene.TEXT_SKIP).text = _settings.SkipLabel.GetLocalizedString(button);
        }

        public void Show()
            => _container.RemoveFromClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);

        public void Hide()
            => _container.AddToClassList(LayoutNames.Cutscene.ACTION_HIDDEN_CLASS_NAME);
    }
}
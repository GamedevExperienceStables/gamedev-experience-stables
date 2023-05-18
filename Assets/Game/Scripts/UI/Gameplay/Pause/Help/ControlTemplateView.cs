using UnityEngine;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class ControlTemplateView
    {
        private Label _action;
        private Label _key;
        
        public void Create(VisualElement root)
        {
            _action = root.Q<Label>("action");
            _key = root.Q<Label>("key");
        }

        public void SetData(HelpContentData.Control control)
        {
            _action.text = control.action.GetLocalizedString();
            _key.text = control.key;
        }
    }
}
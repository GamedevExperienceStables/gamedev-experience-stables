using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class TeamView
    {
        private readonly EmployeeViewFactory _employeeFactory;
        private readonly List<EmployeeView> _employeeViews = new();

        private VisualTreeAsset _template;
        private VisualElement _listContent;
        private Label _name;

        public TeamView(EmployeeViewFactory employeeFactory) 
            => _employeeFactory = employeeFactory;

        public void Create(VisualElement container)
        {
            _name = container.Q<Label>("team-name");
            _listContent = container.Q<VisualElement>("team-list-container");
            _template = _listContent.Q<TemplateContainer>("item-template").templateSource;
            
            _listContent.Clear();
        }

        public void SetData(TeamData teamData)
        {
            _name.text = teamData.name;
            
            for (int i = 1; i <= teamData.list.Count; i++)
            {
                TemplateContainer element = _template.Instantiate();
                _listContent.Add(element);
                
                EmployeeView employeeView = _employeeFactory.Create(element, teamData.list[i - 1], i % 2 == 0);
                _employeeViews.Add(employeeView);
            }
        }

        public void Destroy()
        {
            foreach (EmployeeView employeeView in _employeeViews) 
                employeeView.Destroy();
        }
    }
}
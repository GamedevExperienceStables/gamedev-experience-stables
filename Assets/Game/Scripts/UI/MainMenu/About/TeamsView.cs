using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Game.UI
{
    public class TeamsView
    {
        private readonly List<TeamView> _teamViews = new();
        private readonly TeamsSettings _settings;

        private VisualTreeAsset _teamTemplate;
        private VisualElement _listContent;

        private readonly TeamViewFactory _teamFactory;

        public TeamsView(AboutSettings settings, TeamViewFactory teamFactory)
        {
            _settings = settings.Teams;
            _teamFactory = teamFactory;
        }

        public void Create(VisualElement root)
        {
            _listContent = root.Q<VisualElement>("list-content");
            _teamTemplate = _listContent.Q<TemplateContainer>("team-template").templateSource;
            
            _listContent.Clear();

            CreateTeams();
        }

        public void Destroy()
        {
            foreach (TeamView teamView in _teamViews)
                teamView.Destroy();
        }

        private void CreateTeams()
        {
            List<EmployeeData> allEmployees = new();
            foreach (TeamData teamData in _settings.Teams)
            {
                allEmployees.AddRange(teamData.list);
                
                TemplateContainer element = _teamTemplate.Instantiate();
                _listContent.Add(element);

                TeamView teamView = _teamFactory.Create(element, teamData, allEmployees);
                _teamViews.Add(teamView);
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class TeamViewFactory
    {
        private readonly IObjectResolver _resolver;

        public TeamViewFactory(IObjectResolver resolver) 
            => _resolver = resolver;

        public TeamView Create(TemplateContainer element, TeamData teamData, List<EmployeeData> allEmployees)
        {
            var view = _resolver.Resolve<TeamView>();
            
            view.Create(element);
            view.SetData(teamData, allEmployees);

            return view;
        }
    }
}
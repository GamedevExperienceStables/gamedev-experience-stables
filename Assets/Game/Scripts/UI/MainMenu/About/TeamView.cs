using System.Collections.Generic;
using Unity.Content;
using UnityEngine.UIElements;

namespace Game.UI.About
{
    public class TeamView
    {
        private Label name;
        private List<EmployeeView> _employeeViews;
        
        private void InitTeams()
        {
            for (int i = 1; i <= _employeeViews.Count; i++)
            {
                if (i%2 == 0)
                {
                    _listContent.Add(_employeeViews[i]); 
                    // And add styles to _employeeViews[i] elements;
                }

                _listContent.Add(_employeeViews[i]);
            }
        }
    }
}
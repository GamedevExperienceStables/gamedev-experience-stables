using System.Collections.Generic;
using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class EmployeeViewFactory
    {
        private readonly IObjectResolver _resolver;

        public EmployeeViewFactory(IObjectResolver resolver) 
            => _resolver = resolver;
        
        public EmployeeView Create(TemplateContainer element, EmployeeData employeeData, List<EmployeeData> allEmployees)
        {
            var employeeView = _resolver.Resolve<EmployeeView>();
            
            employeeView.Create(element);
            employeeView.SetData(employeeData, allEmployees.IndexOf(employeeData) % 2 == 0);

            return employeeView;
        }
    }
}
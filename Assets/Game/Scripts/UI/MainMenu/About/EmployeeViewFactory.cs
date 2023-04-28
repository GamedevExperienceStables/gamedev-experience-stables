﻿using UnityEngine.UIElements;
using VContainer;

namespace Game.UI
{
    public class EmployeeViewFactory
    {
        private readonly IObjectResolver _resolver;

        public EmployeeViewFactory(IObjectResolver resolver) 
            => _resolver = resolver;
        
        public EmployeeView Create(TemplateContainer element, EmployeeData employeeData, bool isEven)
        {
            var employeeView = _resolver.Resolve<EmployeeView>();
            
            employeeView.Create(element);
            employeeView.SetData(employeeData, isEven);

            return employeeView;
        }
    }
}
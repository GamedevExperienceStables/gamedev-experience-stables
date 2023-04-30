using UnityEngine;
using VContainer;

namespace Game.UI.About
{
    public class EmployeeViewModel
    {
        private readonly ModalService _modal;
        
        [Inject]
        public EmployeeViewModel(ModalService modal)
        {
            _modal = modal;
        }

        public void ShowModal(ModalContext context) 
            => _modal.Request(context);
        
        public void Link(string url) 
            => Application.OpenURL(url);
    }
}
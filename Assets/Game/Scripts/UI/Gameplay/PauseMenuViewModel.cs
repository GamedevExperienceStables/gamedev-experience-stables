using System;
using Game.GameFlow;

namespace Game.UI
{
    public class PauseMenuViewModel
    {
        private readonly GameplayViewModel _gameplay;
        private readonly ModalService _modal;
        private readonly GameplayMenuInput _menuInput;

        public PauseMenuViewModel(GameplayViewModel gameplay, ModalService modal, GameplayMenuInput menuInput)
        {
            _gameplay = gameplay;
            _modal = modal;
            _menuInput = menuInput;
        }
        
        public bool IsModalOpen => _modal.IsOpen;

        public void ResumeGame()
            => _gameplay.ResumeGame();

        public void GoToMainMenu()
            => _gameplay.GoToMainMenu();

        public void ShowModal(ModalContext context)
            => _modal.Request(context);

        public void SubscribeBack(Action callback) 
            => _menuInput.SubscribeBack(callback);

        public void UnSubscribeBack(Action callback) 
            => _menuInput.UnSubscribeBack(callback);

        public void CloseModal() 
            => _modal.ForceClose();
    }
}
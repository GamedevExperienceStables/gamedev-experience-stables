﻿using Game.Input;
using Game.UI;
using VContainer;

namespace Game.GameFlow
{
    public class PlanetPlayState : GameState
    {
        private readonly GameplayView _view;
        private readonly IInputService _inputService;

        [Inject]
        public PlanetPlayState(
            GameplayView view,
            IInputService inputService
        )
        {
            _view = view;
            _inputService = inputService;
        }

        protected override void OnEnter()
        {
            _inputService.EnableGameplay();
            _view.ShowHud();
        }

        protected override void OnExit()
        {
        }
    }
}
using System;
using VContainer;

namespace Game.Input
{
    public class InputService : IInputService
    {
        private const int MAX_HISTORY_LENGHT = 6;

        private readonly GameInputControlsAdapter _gameInput;

        private readonly InputHistory<InputSchemeGame> _history;

        [Inject]
        public InputService(GameInputControlsAdapter gameInput)
        {
            _gameInput = gameInput;

            _history = new InputHistory<InputSchemeGame>(MAX_HISTORY_LENGHT, InputSchemeGame.None);
        }

        public void PushState(InputSchemeGame state)
        {
            _history.Push(state);
            UpdateInputs(state);
        }

        public void ReplaceState(InputSchemeGame state)
        {
            _history.Replace(state);
            UpdateInputs(state);
        }

        public void Back(int depth = 1)
        {
            _history.Back(depth);

            UpdateInputs(_history.Current);
        }

        private void UpdateInputs(InputSchemeGame scheme)
        {
            switch (scheme)
            {
                case InputSchemeGame.None:
                    _gameInput.GameplayDisable();
                    _gameInput.MenuDisable();
                    break;

                case InputSchemeGame.Gameplay:
                    _gameInput.GameplayEnable();
                    _gameInput.MenuDisable();
                    break;

                case InputSchemeGame.Menu:
                    _gameInput.GameplayDisable();
                    _gameInput.MenuEnable();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null);
            }
        }
    }
}
using System;
using Game.Input;
using VContainer;

namespace Game.GameFlow
{
    public sealed class GameplayBackButton : IDisposable
    {
        private readonly IInputControlGameplay _inputGameplay;
        private readonly IInputControlMenu _inputMenu;
        private bool _menuEnabled;
        private bool _gameplayEnabled;

        private event InputSchemaDelegate BackRequested;

        [Inject]
        public GameplayBackButton(IInputControlGameplay inputGameplay, IInputControlMenu inputMenu)
        {
            _inputGameplay = inputGameplay;
            _inputMenu = inputMenu;

            InputsSubscribe();
        }

        public void Dispose()
            => InputsUnSubscribe();

        public void Subscribe(InputSchemaDelegate callback)
            => BackRequested += callback;

        public void UnSubscribe(InputSchemaDelegate callback)
            => BackRequested -= callback;

        public void SetActive(InputSchema schema, bool isActive)
        {
            switch (schema)
            {
                case InputSchema.Menus:
                    _menuEnabled = isActive;
                    break;

                case InputSchema.Gameplay:
                    _gameplayEnabled = isActive;
                    break;

                case InputSchema.Undefined:
                default:
                    throw new ArgumentOutOfRangeException(nameof(schema), schema, null);
            }
        }

        private void InputsSubscribe()
        {
            _inputGameplay.MenuButton.Performed += OnGameplayBack;

            _inputMenu.CancelButton.Performed += OnMenusBack;
            _inputMenu.ExitButton.Performed += OnMenusBack;
        }

        private void InputsUnSubscribe()
        {
            _inputGameplay.MenuButton.Performed -= OnGameplayBack;

            _inputMenu.CancelButton.Performed -= OnMenusBack;
            _inputMenu.ExitButton.Performed -= OnMenusBack;
        }

        private void OnGameplayBack()
        {
            if (_gameplayEnabled)
                BackRequested?.Invoke(InputSchema.Gameplay);
        }

        private void OnMenusBack()
        {
            if (_menuEnabled)
                BackRequested?.Invoke(InputSchema.Menus);
        }
    }
}
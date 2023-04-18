using System.Collections.Generic;
using UnityEngine.InputSystem;
using VContainer;

namespace Game.Input
{
    public class InputBindings
    {
        private const string KEY_PREFIX = "Key_";

        private readonly Dictionary<InputGameplayActions, string> _keyBindings = new();

        private readonly GameInputControls.GameplayActions _gameplayActions;
        private readonly GameInputControls.MenuActions _menuActions;


        [Inject]
        public InputBindings(GameInputControlsAdapter adapter)
        {
            _gameplayActions = adapter.GetGameplayActions();
            _menuActions = adapter.GetMenuActions();

            InputControlScheme keyboardScheme = adapter.GetKeyboardMouseScheme();
            BuildKeyBindings(keyboardScheme.bindingGroup);
        }

        private void BuildKeyBindings(string group)
        {
            _keyBindings.Add(InputGameplayActions.Back, GetInputKey(_menuActions.Back, group));
            _keyBindings.Add(InputGameplayActions.Interaction, GetInputKey(_gameplayActions.Interaction, group));
            _keyBindings.Add(InputGameplayActions.Slot1, GetInputKey(_gameplayActions.Slot1, group));
            _keyBindings.Add(InputGameplayActions.Slot2, GetInputKey(_gameplayActions.Slot2, group));
            _keyBindings.Add(InputGameplayActions.Slot3, GetInputKey(_gameplayActions.Slot3, group));
            _keyBindings.Add(InputGameplayActions.Slot4, GetInputKey(_gameplayActions.Slot4, group));
            _keyBindings.Add(InputGameplayActions.Fire, GetInputKey(_gameplayActions.Fire, group));
            _keyBindings.Add(InputGameplayActions.Inventory, GetInputKey(_gameplayActions.Inventory, group));
        }

        private static string GetInputKey(InputAction action, string group)
        {
            int index = action.GetBindingIndex(group);
            string fallback = action.GetBindingDisplayString(index, out string deviceLayout, out string controlPath);

            if (string.IsNullOrEmpty(deviceLayout) || string.IsNullOrEmpty(controlPath))
                return fallback;

            return deviceLayout switch
            {
                "Keyboard" => ToSpriteKey(controlPath),
                "Mouse" => ToSprite(controlPath),
                _ => fallback
            };
        }

        public string GetBindingDisplayString(InputGameplayActions inputAction)
            => _keyBindings[inputAction];

        private static string ToSprite(string key)
            => $"<sprite name=\"{key}\">";

        private static string ToSpriteKey(string key)
            => ToSprite($"{KEY_PREFIX}{key}");
    }
}
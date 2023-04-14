using System;
using Game.Input;
using UnityEngine.Localization;
using VContainer;

namespace Game.Level
{
    public class LocalizationInteraction
    {
        private readonly LocalizationInteractionSettings _settings;
        private readonly InputBindings _bindings;

        [Inject]
        public LocalizationInteraction(LocalizationInteractionSettings settings, InputBindings bindings)
        {
            _settings = settings;
            _bindings = bindings;
        }

        public string GetText(Interaction interaction)
        {
            return interaction switch
            {
                LevelExitInteraction => HandleLevelExit(interaction),
                RocketContainerInteraction container => HandleContainer(container),
                TransitionToLocationInteraction transition => HandleTransition(transition),
                SaveGameInteraction => GetInteractionText(_settings.SaveGame.Text),
                _ => GetInteractionText(_settings.DefaultText)
            };
        }

        private string HandleTransition(TransitionToLocationInteraction transition)
        {
            if (transition.TargetLocation == _settings.Transition.PlanetLocation)
                return GetInteractionText(_settings.Transition.TextToPlanet);

            if (transition.TargetLocation == _settings.Transition.RocketLocation)
                return GetInteractionText(_settings.Transition.TextToRocket);

            return GetInteractionText(_settings.Transition.TextDoor);
        }

        private string HandleLevelExit(Interaction interaction)
        {
            return interaction.CanExecute()
                ? GetInteractionText(_settings.LevelExit.TextTrue)
                : GetText(_settings.LevelExit.TextFalse);
        }

        private string HandleContainer(RocketContainerInteraction container)
        {
            container.CanExecuteWithResult(out InteractionRocketResult result);
            return result switch
            {
                InteractionRocketResult.Ok => GetInteractionText(_settings.RocketContainer.TextOk),
                InteractionRocketResult.Empty => GetText(_settings.RocketContainer.TextEmpty),
                InteractionRocketResult.Full => GetText(_settings.RocketContainer.TextFull),
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };
        }

        private static string GetText(LocalizedString localizedString)
        {
            return localizedString is null
                ? string.Empty
                : localizedString.GetLocalizedString();
        }

        private string GetInteractionText(LocalizedString text)
        {
            string button = _bindings.GetBindingDisplayString(InputGameplayActions.Interaction);
            return text.GetLocalizedString(button);
        }
    }
}
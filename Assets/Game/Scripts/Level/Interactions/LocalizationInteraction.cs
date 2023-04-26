using System;
using UnityEngine.Localization;
using VContainer;

namespace Game.Level
{
    public class LocalizationInteraction
    {
        private readonly LocalizationInteractionSettings _settings;

        [Inject]
        public LocalizationInteraction(LocalizationInteractionSettings settings)
            => _settings = settings;

        public InteractionPrompt GetInteractionPrompt(Interaction interaction) =>
            interaction switch
            {
                LevelExitInteraction => HandleLevelExit(interaction),
                RocketContainerInteraction container => HandleContainer(container),
                TransitionToLocationInteraction transition => HandleTransition(transition),
                SaveGameInteraction => GetCommonPrompt(_settings.SaveGame.Text),
                _ => GetCommonPrompt(_settings.DefaultText)
            };

        private static InteractionPrompt GetCommonPrompt(LocalizedString saveGameText)
        {
            string localizedText = saveGameText.GetLocalizedString();
            return new InteractionPrompt(localizedText, canExecute: true);
        }

        private InteractionPrompt HandleTransition(TransitionToLocationInteraction transition)
        {
            string localizedText = GetLocalizedText(_settings.Transition.TextDoor);

            if (transition.TargetLocation == _settings.Transition.PlanetLocation)
                localizedText = GetLocalizedText(_settings.Transition.TextToPlanet);

            if (transition.TargetLocation == _settings.Transition.RocketLocation)
                localizedText = GetLocalizedText(_settings.Transition.TextToRocket);

            return new InteractionPrompt(localizedText, canExecute: true);
        }

        private InteractionPrompt HandleLevelExit(Interaction interaction)
        {
            bool canExecute = interaction.CanExecute();

            string localizedText = canExecute
                ? GetLocalizedText(_settings.LevelExit.TextTrue)
                : GetText(_settings.LevelExit.TextFalse);

            return new InteractionPrompt(localizedText, canExecute);
        }

        private InteractionPrompt HandleContainer(RocketContainerInteraction container)
        {
            bool canExecute = container.CanExecuteWithResult(out InteractionRocketResult result);

            string localizedText = result switch
            {
                InteractionRocketResult.Ok => GetLocalizedText(_settings.RocketContainer.TextOk),
                InteractionRocketResult.Empty => GetText(_settings.RocketContainer.TextEmpty),
                InteractionRocketResult.Full => GetText(_settings.RocketContainer.TextFull),
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };

            return new InteractionPrompt(localizedText, canExecute);
        }

        private static string GetText(LocalizedString localizedString)
        {
            return localizedString is null
                ? string.Empty
                : localizedString.GetLocalizedString();
        }

        private static string GetLocalizedText(LocalizedString text)
            => text.GetLocalizedString();
    }
}
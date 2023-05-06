using System;
using Game.Inventory;
using Game.Settings;
using UnityEngine.Localization;

namespace Game.Level
{
    public class RocketContainerLocalization
    {
        private readonly LocalizationInteractionSettings _settings;
        private readonly LevelController _level;
        private readonly IInventoryItems _inventory;

        public RocketContainerLocalization(LocalizationInteractionSettings settings, LevelController level, IInventoryItems inventory)
        {
            _settings = settings;
            _level = level;
            _inventory = inventory;
        }

        public InteractionPrompt GetPrompt(RocketContainerInteraction container)
        {
            bool canExecute = container.CanExecuteWithResult(out InteractionRocketResult result);

            string localizedText = result switch
            {
                InteractionRocketResult.Ok => GetTransferText(_settings.RocketContainer.TextOk, container.Material),
                InteractionRocketResult.Empty => GetRequirementsText(_settings.RocketContainer.TextEmpty, container.Material),
                InteractionRocketResult.Full => GetText(_settings.RocketContainer.TextFull),
                _ => throw new ArgumentOutOfRangeException(nameof(result), result, null)
            };

            return new InteractionPrompt(localizedText, canExecute);
        }

        private string GetTransferText(LocalizedString rocketContainerTextOk, MaterialDefinition containerMaterial)
        {
            string materialName = containerMaterial.LocalizedName.GetLocalizedString();
            int currentValue = _inventory.Materials.Bag.GetCurrentValue(containerMaterial);
            
            return rocketContainerTextOk.GetLocalizedString(materialName, currentValue.ToString());
        }

        private static string GetText(LocalizedString text) 
            => text.GetLocalizedString();

        private string GetRequirementsText(LocalizedString localizedString, MaterialDefinition material)
        {
            LevelDefinition currentLevel = _level.GetCurrentLevel();
            int target = currentLevel.Goal.Count;
            
            int currentValue = _inventory.Materials.Container.GetCurrentValue(material);
            int remainCount = Math.Max(0, target - currentValue);

            string materialName = material.LocalizedName.GetLocalizedString();
            return localizedString.GetLocalizedString(materialName, remainCount.ToString());
        }
    }
}
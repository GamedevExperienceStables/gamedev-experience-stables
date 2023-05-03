using System;
using Game.Inventory;
using Game.Level;
using UnityEngine.Localization;
using VContainer;
using VContainer.Unity;

namespace Game.Dialog
{
    public sealed class DialogNotification : IStartable, IDisposable
    {
        private readonly Settings _settings;
        private readonly InventoryController _inventory;
        private readonly DialogService _dialog;

        [Inject]
        public DialogNotification(Settings settings, InventoryController inventory, DialogService dialog)
        {
            _settings = settings;
            _inventory = inventory;
            _dialog = dialog;
        }

        public void Start()
        {
            _inventory.Runes.Subscribe(OnRuneAdded);
            _inventory.Materials.Bag.Subscribe(OnMaterialAdded);
        }

        public void Dispose()
        {
            _inventory.Runes.UnSubscribe(OnRuneAdded);
            _inventory.Materials.Bag.UnSubscribe(OnMaterialAdded);
        }

        private void OnMaterialAdded(MaterialChangedData change)
        {
            if (change.newValue <= change.oldValue)
                return;

            string localizedString = _settings.materialObtained.GetLocalizedString(change.definition);
            
            ShowDialog(localizedString);
        }

        private void OnRuneAdded(RuneDefinition rune)
        {
            string runeName = rune.Name.GetLocalizedString();
            string localizedString = _settings.runeObtained.GetLocalizedString(runeName);
            
            ShowDialog(localizedString);
        }

        private void ShowDialog(string text)
        {
            DialogData dialogData = CreateDialog(text);
            _dialog.Show(dialogData);
        }

        private static DialogData CreateDialog(string text)
        {
            var item = new DialogItem(text);
            var dialog = new Level.Dialog(item);
            var dialogData = new DialogData(dialog, oneShot: true);
            return dialogData;
        }


        [Serializable]
        public class Settings
        {
            public LocalizedString runeObtained;
            public LocalizedString materialObtained;
        }
    }
}
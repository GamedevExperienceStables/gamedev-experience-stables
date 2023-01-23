using Game.Actors;
using Game.Inventory;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Level
{
    [UsedImplicitly]
    public class ItemPickupInteraction : Interaction
    {
        private ItemDefinition _item;
        private IActorController _instigator;

        public void Init(ItemDefinition item, IActorController instigator, GameObject source)
        {
            Source = source;

            _instigator = instigator;
            _item = item;
        }

        public override bool CanExecute()
            => true;

        public override void Execute()
        {
        }
    }
}
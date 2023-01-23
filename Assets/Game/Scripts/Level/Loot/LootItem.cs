using Game.Inventory;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Level
{
    public class LootItem : Interactable
    {
        [SerializeField]
        private MMF_Player pickupFeedback;

        public ItemDefinition Definition { get; private set; }

        public void Init(ItemDefinition definition) 
            => Definition = definition;
    }
}
using Game.Inventory;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Level
{
    [DisallowMultipleComponent]
    public class RocketContainer : Interactable
    {
        [SerializeField]
        private MMF_Player transferFeedback;

        [SerializeField]
        private MaterialDefinition targetMaterial;

        public MaterialDefinition TargetMaterial => targetMaterial;
        
        public void TransferCompleted()
        {
            if (transferFeedback)
                transferFeedback.PlayFeedbacks();
        }
    }
}
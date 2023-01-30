using MoreMountains.Feedbacks;
using UnityEngine;

namespace Game.Level
{
    [DisallowMultipleComponent]
    public class RocketContainer : Interactable
    {
        [SerializeField]
        private MMF_Player transferFeedback;

        public void TransferCompleted()
        {
            if (transferFeedback)
                transferFeedback.PlayFeedbacks();
        }
    }
}
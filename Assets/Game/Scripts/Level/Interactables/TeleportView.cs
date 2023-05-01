using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    public class TeleportView : MonoBehaviour
    {
        [SerializeField, Required]
        private LocationDoor door;

        [SerializeField, Required]
        private GameObject teleportOutFeedback;
        
        private void Start()
            => door.TransitionStart += OnTransitionStart;

        private void OnDestroy()
            => door.TransitionStart -= OnTransitionStart;

        private void OnTransitionStart() 
            => teleportOutFeedback.SetActive(true);
    }
}
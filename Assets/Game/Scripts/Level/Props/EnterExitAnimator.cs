using Game.Animations.Hero;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class EnterExitAnimator : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private ZoneTrigger _zoneTrigger;

        private void Awake()
        {
            _zoneTrigger = GetComponent<ZoneTrigger>();
            _zoneTrigger.TriggerEntered += OnTriggerEntered;
            _zoneTrigger.TriggerExited += OnTriggerExited;
        }

        private void OnDestroy()
        {
            _zoneTrigger.TriggerEntered -= OnTriggerEntered;
            _zoneTrigger.TriggerExited -= OnTriggerExited;
        }

        private void OnTriggerEntered(GameObject obj)
            => animator.SetBool(AnimationNames.IsActive, true);

        private void OnTriggerExited(GameObject obj)
            => animator.SetBool(AnimationNames.IsActive, false);
    }
}
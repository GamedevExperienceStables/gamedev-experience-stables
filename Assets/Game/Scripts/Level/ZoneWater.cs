using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class ZoneWater : MonoBehaviour
    {
        [SerializeField]
        private GameObject enterEffect;

        private ZoneTrigger _trigger;

        private void Awake()
            => _trigger = GetComponent<ZoneTrigger>();

        private void OnEnable()
            => _trigger.TriggerEntered += OnTriggerEntered;

        private void OnDisable()
            => _trigger.TriggerEntered -= OnTriggerEntered;

        private void OnTriggerEntered(GameObject other)
            => Instantiate(enterEffect, other.transform.position, Quaternion.identity);
    }
}
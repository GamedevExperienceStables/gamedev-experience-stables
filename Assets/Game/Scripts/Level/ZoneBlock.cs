using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    [RequireComponent(typeof(ZoneTrigger))]
    public class ZoneBlock : MonoBehaviour
    {
        [SerializeField]
        private InputBlock block = (InputBlock)~0;

        private ZoneTrigger _trigger;
        private ComponentCollector<IActorInputController> _collector;

        private void Awake()
        {
            _trigger = GetComponent<ZoneTrigger>();
            _collector = new ComponentCollector<IActorInputController>();
        }

        private void OnEnable()
        {
            _trigger.TriggerEntered += OnTriggerEntered;
            _trigger.TriggerExited += OnTriggerExited;
        }

        private void OnDisable()
        {
            _trigger.TriggerEntered -= OnTriggerEntered;
            _trigger.TriggerExited -= OnTriggerExited;
        }

        private void OnDestroy()
        {
            foreach (IActorInputController actor in _collector.Items)
                actor.RemoveBlock(block);

            _collector.Clear();
        }

        private void OnTriggerEntered(GameObject other)
        {
            if (_collector.TryAdd(other, out IActorInputController actor))
                actor.SetBlock(block);
        }

        private void OnTriggerExited(GameObject obj)
        {
            if (_collector.TryRemove(obj, out IActorInputController actor))
                actor.RemoveBlock(block);
        }
    }
}
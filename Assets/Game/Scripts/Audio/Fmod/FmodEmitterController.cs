using FMODUnity;
using UnityEngine;
using VContainer;

namespace Game.Audio
{
    [RequireComponent(typeof(StudioEventEmitter))]
    public class FmodEmitterController : MonoBehaviour
    {
        private StudioEventEmitter _emitter;
        private FmodService _fmod;

        [Inject]
        public void Construct(FmodService fmod)
            => _fmod = fmod;

        private void Awake()
        {
            _emitter = GetComponent<StudioEventEmitter>();
            CustomStartEvent();
        }

        private void OnDestroy()
            => _emitter.Stop();

        private void CustomStartEvent()
        {
            if (_emitter.PlayEvent != EmitterGameEvent.ObjectStart)
                return;

            _emitter.PlayEvent = EmitterGameEvent.None;
            _fmod?.RegisterOnStart(_emitter);
        }
    }
}
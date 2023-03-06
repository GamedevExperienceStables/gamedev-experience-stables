using Game.Audio;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Game.Actors
{
    [RequireComponent(typeof(MovementController))]
    public class AnimationFootstepsEmitter : MonoBehaviour
    {
        private MovementController _movement;
        private FootstepsEmitter _emitter;

        [Inject]
        public void Construct(FootstepsEmitter emitter)
        {
            _emitter = emitter;
            _movement = GetComponent<MovementController>();
        }

        [UsedImplicitly]
        private void AnimationStepEvent()
            => _emitter.TryEmitAudio(_movement.Velocity, transform.position);
    }
}
using UnityEngine;
using VContainer;

namespace Game.Audio
{
    public class FootstepsEmitter
    {
        private const float RAYCAST_DISTANCE = 0.5f;
        private const float THRESHOLD = 0.1f;
        private readonly Vector3 _raycastOffset = new(0, 0.1f, 0f);

        private readonly IFootstepsAudio _audio;

        [Inject]
        public FootstepsEmitter(IFootstepsAudio footstepsAudio)
            => _audio = footstepsAudio;

        public void TryEmitAudio(Vector3 velocity, Vector3 position)
        {
            if (!IsValidSpeed(velocity))
                return;

            if (!HasSurface(position, out Transform surface))
                return;

            SurfaceType surfaceType = GetMaterialType(surface);
            _audio.Play(surfaceType, position);
        }

        private static bool IsValidSpeed(Vector3 velocity)
            => velocity.sqrMagnitude >= THRESHOLD;

        private bool HasSurface(Vector3 position, out Transform surface)
        {
            if (Physics.Raycast(position + _raycastOffset, Vector3.down, out RaycastHit hit, RAYCAST_DISTANCE))
            {
                surface = hit.transform;
                return true;
            }

            surface = default;
            return false;
        }

        private static SurfaceType GetMaterialType(Component surface)
        {
            SurfaceType surfaceType = default;

            if (surface.TryGetComponent(out SurfaceMaterialObject material))
                surfaceType = material.Type;

            return surfaceType;
        }
    }
}
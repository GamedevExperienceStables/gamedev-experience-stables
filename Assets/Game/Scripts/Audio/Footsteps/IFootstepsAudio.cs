using UnityEngine;

namespace Game.Audio
{
    public interface IFootstepsAudio
    {
        void Play(SurfaceType type, Vector3 position);
    }
}
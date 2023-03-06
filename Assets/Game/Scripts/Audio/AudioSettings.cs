using System;
using UnityEngine;

namespace Game.Audio
{
    [Serializable]
    public class AudioSettings
    {
        [SerializeField]
        private AudioSurfaceSettings surfaceSettings;

        public AudioSurfaceSettings SurfaceSettings => surfaceSettings;
    }
}
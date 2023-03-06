using System;
using FMODUnity;
using UnityEngine;

namespace Game.Audio
{
    [Serializable]
    public class AudioSurfaceSettings
    {
        [SerializeField]
        private EventReference defaultFootSteps;

        public EventReference DefaultFootsteps => defaultFootSteps;
    }
}
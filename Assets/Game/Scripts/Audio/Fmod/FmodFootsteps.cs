using System;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using VContainer;

namespace Game.Audio
{
    public sealed class FmodFootsteps : IFootstepsAudio, IDisposable
    {
        private readonly FmodService _fmod;

        private readonly EventReference _footsteps;
        private readonly PARAMETER_ID _parameterId;

        private EventDescription _eventDescription;

        [Inject]
        public FmodFootsteps(FmodService fmod, AudioSettings settings)
        {
            _fmod = fmod;
            _footsteps = settings.SurfaceSettings.DefaultFootsteps;

            _eventDescription = fmod.GetEventDescription(_footsteps);
            _eventDescription.loadSampleData();

            _eventDescription.getParameterDescriptionByIndex(0, out PARAMETER_DESCRIPTION parameter);
            _parameterId = parameter.id;
        }

        public void Play(SurfaceType type, Vector3 position)
        {
            EventInstance instance = _fmod.CreateInstance(_footsteps);
            instance.set3DAttributes(position.To3DAttributes());
            instance.setParameterByID(_parameterId, (float)type);
            instance.start();
            instance.release();
        }

        public void Dispose() 
            => _eventDescription.unloadSampleData();
    }
}
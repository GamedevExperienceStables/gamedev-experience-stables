using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using VContainer;
using STOP_MODE = FMOD.Studio.STOP_MODE;

// ReSharper disable MemberCanBeMadeStatic.Global
namespace Game.Audio
{
    public sealed class FmodService : IAudioService, IDisposable
    {
        private readonly List<StudioEventEmitter> _onStartEmitters = new();

        private EventInstance _pauseSnapshot;

        [Inject]
        public FmodService() 
            => _pauseSnapshot = CreateInstance(AudioNames.Snapshot.PAUSE);

        public void PlayOneShot(EventReference eventReference, Transform source)
            => RuntimeManager.PlayOneShot(eventReference, source.position);

        public EventInstance CreateInstance(EventReference eventReference)
            => RuntimeManager.CreateInstance(eventReference);

        public EventDescription GetEventDescription(EventReference eventReference)
            => RuntimeManager.GetEventDescription(eventReference);

        public void StartLocationSounds()
        {
            foreach (StudioEventEmitter emitter in _onStartEmitters)
                emitter.Play();

            _onStartEmitters.Clear();
        }

        public void Pause()
            => _pauseSnapshot.start();

        public void Resume()
            => _pauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);

        public void RegisterOnStart(StudioEventEmitter emitter)
            => _onStartEmitters.Add(emitter);

        private static EventInstance CreateInstance(string path)
            => RuntimeManager.CreateInstance(path);

        public void Dispose()
        {
            _pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
            _pauseSnapshot.release();
        }
    }
}
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
    public sealed class FmodService : IAudioService, IAudioTuner, IDisposable
    {
        private readonly List<StudioEventEmitter> _onStartEmitters = new();

        private EventInstance _pauseSnapshot;

        private VCA _masterVolume;
        private VCA _musicVolume;
        private VCA _effectsVolume;

        [Inject]
        public FmodService()
        {
            _pauseSnapshot = CreateInstance(AudioNames.Snapshot.PAUSE);

            _masterVolume = RuntimeManager.GetVCA(AudioNames.Vca.MASTER);
            _musicVolume = RuntimeManager.GetVCA(AudioNames.Vca.MUSIC);
            _effectsVolume = RuntimeManager.GetVCA(AudioNames.Vca.EFFECTS);
        }

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

        public void SetVolume(AudioChannel channel, float value)
        {
            switch (channel)
            {
                case AudioChannel.Master:
                    _masterVolume.setVolume(value);
                    break;
                case AudioChannel.Music:
                    _musicVolume.setVolume(value);
                    break;
                case AudioChannel.Effects:
                    _effectsVolume.setVolume(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }
        }

        public float GetVolume(AudioChannel channel)
        {
            float volume;
            switch (channel)
            {
                case AudioChannel.Master:
                    _masterVolume.getVolume(out volume);
                    break;
                case AudioChannel.Music:
                    _musicVolume.getVolume(out volume);
                    break;
                case AudioChannel.Effects:
                    _effectsVolume.getVolume(out volume);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(channel), channel, null);
            }

            return volume;
        }

        private static EventInstance CreateInstance(string path)
            => RuntimeManager.CreateInstance(path);

        public void Dispose()
        {
            _pauseSnapshot.stop(STOP_MODE.IMMEDIATE);
            _pauseSnapshot.release();
        }
    }
}
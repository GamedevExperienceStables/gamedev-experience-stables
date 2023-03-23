using Game.Audio;
using Game.Persistence;

namespace Game.Player
{
    public class PlayerAudioPrefs
    {
        private const string VOLUME_KEY = "audio.volume";

        private readonly IPlayerPrefs _playerPrefs;
        private readonly IAudioTuner _audioTuner;

        public PlayerAudioPrefs(IPlayerPrefs playerPrefs, IAudioTuner audioTuner)
        {
            _playerPrefs = playerPrefs;
            _audioTuner = audioTuner;
        }

        public void Init()
        {
            Init(AudioChannel.Master);
            Init(AudioChannel.Effects);
            Init(AudioChannel.Music);
        }

        public void SetVolume(AudioChannel channel, float value)
        {
            string key = GetChannelKey(channel);
            _playerPrefs.SetFloat(key, value);

            _audioTuner.SetVolume(channel, value);
        }

        public float GetVolume(AudioChannel channel)
            => _audioTuner.GetVolume(channel);

        private void Init(AudioChannel audioChannel)
        {
            string key = GetChannelKey(audioChannel);
            float volume = _playerPrefs.GetFloat(key, 1f);

            _audioTuner.SetVolume(audioChannel, volume);
        }

        private static string GetChannelKey(AudioChannel channel)
            => $"{VOLUME_KEY}.{channel.ToString()}";
    }
}
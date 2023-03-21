namespace Game.Audio
{
    public interface IAudioTuner
    {
        void SetVolume(AudioChannel channel, float value);
        float GetVolume(AudioChannel channel);
    }
}
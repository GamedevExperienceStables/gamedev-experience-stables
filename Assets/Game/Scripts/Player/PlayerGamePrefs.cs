namespace Game.Player
{
    public class PlayerGamePrefs
    {
        private readonly PlayerAudioPrefs _audio;
        private readonly PlayerGraphicsPrefs _graphics;

        public PlayerGamePrefs(PlayerAudioPrefs audio, PlayerGraphicsPrefs graphics)
        {
            _audio = audio;
            _graphics = graphics;
        }

        public void Init()
        {
            _audio.Init();
            _graphics.Init();
        }
    }
}
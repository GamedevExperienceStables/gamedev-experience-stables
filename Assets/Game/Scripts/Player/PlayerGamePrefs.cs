namespace Game.Player
{
    public class PlayerGamePrefs
    {
        private readonly PlayerAudioPrefs _audio;
        private readonly PlayerGraphicsPrefs _graphics;
        private readonly PlayerLocalizationPrefs _localization;

        public PlayerGamePrefs(PlayerAudioPrefs audio, PlayerGraphicsPrefs graphics, PlayerLocalizationPrefs localization)
        {
            _audio = audio;
            _graphics = graphics;
            _localization = localization;
        }

        public void Init()
        {
            _audio.Init();
            _graphics.Init();
            _localization.Init();
        }
    }
}
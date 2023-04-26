namespace Game.Level
{
    public readonly struct InteractionPrompt
    {
        public readonly string text;
        public readonly bool canExecute;

        public InteractionPrompt(string text, bool canExecute)
        {
            this.text = text;
            this.canExecute = canExecute;
        }
    }
}
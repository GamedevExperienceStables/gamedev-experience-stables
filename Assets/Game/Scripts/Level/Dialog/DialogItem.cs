namespace Game.Level
{
    public class DialogItem : IDialogItem
    {
        public DialogItem(string text)
        {
            Text = text;
            Title = string.Empty;
        }

        public DialogItem(string text, string title)
        {
            Text = text;
            Title = title;
        }

        public string Text { get; }
        public string Title { get; }
    }
}
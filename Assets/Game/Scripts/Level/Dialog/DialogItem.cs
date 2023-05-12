using UnityEngine;

namespace Game.Level
{
    public class DialogItem : IDialogItem
    {
        public DialogItem(string text)
        {
            Text = text;
            
            Title = string.Empty;
            Image = default;
        }

        public string Text { get; }
        public string Title { get; }
        public Sprite Image { get; }
    }
}
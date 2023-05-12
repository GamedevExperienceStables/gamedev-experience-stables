using UnityEngine;

namespace Game.Level
{
    public interface IDialogItem
    {
        string Title { get; }
        string Text { get; }
        
        Sprite Image { get; }
    }
}
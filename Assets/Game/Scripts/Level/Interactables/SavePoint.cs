using System;

namespace Game.Level
{
    public class SavePoint : Interactable
    {
        public event Action Saving;

        public void Executed() 
            => Saving?.Invoke();
    }
}
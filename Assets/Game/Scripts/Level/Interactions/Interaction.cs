using UnityEngine;

namespace Game.Level
{
    public abstract class Interaction
    {
        public GameObject Source { get; protected set; }

        public abstract bool CanExecute();
        public abstract void Execute();
    }
}
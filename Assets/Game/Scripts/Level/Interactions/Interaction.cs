using UnityEngine;

namespace Game.Level
{
    public abstract class Interaction
    {
        public GameObject Source { get; protected set; }

        public abstract void Execute();
    }
}
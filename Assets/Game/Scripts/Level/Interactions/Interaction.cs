using Game.Actors;
using UnityEngine;

namespace Game.Level
{
    public abstract class Interaction
    {
        public GameObject Source { get; set; }
        public IActorController Instigator { get; set; }

        public abstract void OnCreate();
        public abstract bool CanExecute();
        public abstract void Execute();
    }
}
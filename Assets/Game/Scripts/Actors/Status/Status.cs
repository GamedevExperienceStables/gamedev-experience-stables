using UnityEngine;

namespace Game.Actors
{
    public class Status
    {
        public Status(StatusDefinition status)
        {
            Count = 1;
            Definition = status;
            
            HasView = false;
            View = default;
        }

        public Status(StatusDefinition status, GameObject view)
        {
            Count = 1;
            Definition = status;
            
            HasView = true;
            View = view;
        }

        public bool HasView { get; }
        public GameObject View { get; }
        public StatusDefinition Definition { get; }

        public int Count { get; private set; }

        public void Increase() 
            => Count++;

        public void Decrease() 
            => Count--;
    }
}
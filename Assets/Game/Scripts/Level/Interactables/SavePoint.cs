using System;
using UnityEngine;

namespace Game.Level
{
    public class SavePoint : Interactable
    {
        [SerializeField]
        private LocationPoint spawnPoint;
        
        public event Action Saving;

        public ILocationPointKey PointKey => spawnPoint.PointKey;

        public void Executed() 
            => Saving?.Invoke();
    }
}
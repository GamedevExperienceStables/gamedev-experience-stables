using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Level
{
    [DisallowMultipleComponent]
    public class LocationDoor : Interactable
    {
        [FormerlySerializedAs("targetConnection")]
        [SerializeField, Required("Select LocationPoint where player will be spawned")]
        private LocationPointDefinition targetLocationPoint;

        public LocationPointDefinition TargetLocationPoint => targetLocationPoint;
        
        public event Action TransitionStart; 

        public void Transition() 
            => TransitionStart?.Invoke();
    }
}
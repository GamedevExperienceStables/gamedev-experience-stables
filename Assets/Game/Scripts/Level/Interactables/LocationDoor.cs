using UnityEngine;

namespace Game.Level
{
    [DisallowMultipleComponent]
    public class LocationDoor : Interactable
    {
        [SerializeField]
        private LocationPointDefinition targetConnection;

        public LocationPointDefinition TargetLocation => targetConnection;
    }
}
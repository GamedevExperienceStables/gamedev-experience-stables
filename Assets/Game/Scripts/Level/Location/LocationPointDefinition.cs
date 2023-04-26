using UnityEngine;

namespace Game.Level
{
    public abstract class LocationPointDefinition : ScriptableObject, ILocationPointKeyOwner
    {
        [SerializeField]
        protected LocationPointKey locationPointKey;

        public ILocationPointKey PointKey => locationPointKey;
    }
}
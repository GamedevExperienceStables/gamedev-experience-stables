using UnityEngine;

namespace Game.Level
{
    public abstract class LocationPointDefinition : ScriptableObject
    {
        [SerializeField]
        protected LocationPointKey locationPointKey;

        public LocationPointKey PointKey => locationPointKey;
    }
}
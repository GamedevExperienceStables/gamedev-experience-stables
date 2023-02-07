using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    public partial class LocationPoint : MonoBehaviour, ILocationPointKeyOwner
    {
        [SerializeField, Required("Select SpawnPointKey for this location point.")]
        private LocationPointKey point;

        public ILocationPointKey PointKey => point;
    }
}
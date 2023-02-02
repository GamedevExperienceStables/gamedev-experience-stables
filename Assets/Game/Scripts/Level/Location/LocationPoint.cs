using UnityEngine;

namespace Game.Level
{
    public partial class LocationPoint : MonoBehaviour, ILocationPointKeyOwner
    {
        [SerializeField]
        private LocationPointKey point;

        public ILocationPointKey PointKey => point;
    }
}
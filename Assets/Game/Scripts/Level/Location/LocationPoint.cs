using UnityEngine;

namespace Game.Level
{
    public class LocationPoint : MonoBehaviour
    {
        [SerializeField]
        private LocationPointKey point;

        public LocationPointKey Key => point;
    }
}
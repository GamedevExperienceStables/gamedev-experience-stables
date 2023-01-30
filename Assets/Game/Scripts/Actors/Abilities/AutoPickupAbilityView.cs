using Game.Level;
using UnityEngine;

namespace Game.Actors
{
    public class AutoPickupAbilityView : MonoBehaviour
    {
        [SerializeField]
        private ZoneTrigger trigger;

        public ZoneTrigger Trigger => trigger;
    }
}
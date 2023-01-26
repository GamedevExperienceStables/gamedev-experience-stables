using Game.Level;
using UnityEngine;

namespace Game.Actors
{
    public class AutoPickupAbilityView : MonoBehaviour
    {
        [field: SerializeField]
        public ZoneTrigger Trigger { get; private set; }
    }
}
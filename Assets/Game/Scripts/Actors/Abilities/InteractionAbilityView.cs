using Game.Level;
using UnityEngine;

namespace Game.Actors
{
    public class InteractionAbilityView : MonoBehaviour
    {
        [SerializeField]
        private ZoneTrigger trigger;

        public ZoneTrigger Trigger => trigger;
    }
}
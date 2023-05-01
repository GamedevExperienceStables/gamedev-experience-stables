using UnityEngine;

namespace Game.Actors
{
    [CreateAssetMenu(menuName = "★ FX/MeleeAbility")]
    public class MeleeAbilityFx : ScriptableObject
    {
        [SerializeField]
        private GameObject onStart;

        [SerializeField]
        private GameObject onEnd;

        [Space]
        [SerializeField]
        private GameObject onHit;

        [SerializeField]
        private Vector3 onHitOffset;

        public GameObject OnStart => onStart;
        public GameObject OnEnd => onEnd;
        public GameObject OnHit => onHit;
        public Vector3 OnHitOffset => onHitOffset;
    }
}
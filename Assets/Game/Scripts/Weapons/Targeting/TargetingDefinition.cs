using NaughtyAttributes;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Projectile/Targeting")]
    public class TargetingDefinition : ScriptableObject, ITargetingSettings
    {
        [Header("Grounding")]
        [SerializeField]
        private Vector3 targetPositionOffset;

        [SerializeField]
        private bool relativeToGround = true;

        [Header("Restriction")]
        [SerializeField, Min(0)]
        private float minDistanceToTarget = 0.2f;
        
        [SerializeField]
        private bool allowTargetAbove;

        [SerializeField]
        private bool allowTargetBelow;

        [SerializeField, Min(0)]
        [ShowIf(nameof(allowTargetBelow))]
        private float minDistanceToTargetBelow;

        public Vector3 TargetPositionOffset => targetPositionOffset;

        public bool RelativeToGround => relativeToGround;

        public bool AllowTargetAbove => allowTargetAbove;

        public bool AllowTargetBelow => allowTargetBelow;

        public float MinDistanceToTargetBelow => minDistanceToTargetBelow;

        public float MinDistanceToTarget => minDistanceToTarget;
    }
}
using Game.Level;
using Game.Utils;
using NaughtyAttributes;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = "Damage/Trap")]
    public class DamageDefinitionTrap : DamageDefinition
    {
        [SerializeField, Expandable]
        private TrapDefinition spawn;

        [SerializeField]
        private LayerMask verticalCollisionLayers;
        
        [SerializeField, Min(0f)]
        private float maxGroundDistance = 2f;
        
        [SerializeField, Range(0f, 90f)]
        private float maxGroundAngle = 20f;

        [SerializeField]
        private bool snapToGround = true;

        [SerializeField]
        private bool alignToGround = true;

        [SerializeField]
        private bool faceInDirection;

        public LayerMask GroundLayers => LayerMasks.GroundLayers;

        public bool AlignToGround => alignToGround;

        public bool SnapToGround => snapToGround;

        public TrapDefinition SpawnDefinition => spawn;

        public bool FaceInDirection => faceInDirection;

        public LayerMask VerticalCollisionLayers => verticalCollisionLayers;
        public float MaxGroundDistance => maxGroundDistance;
        public float MaxGroundAngle => maxGroundAngle;
    }
}
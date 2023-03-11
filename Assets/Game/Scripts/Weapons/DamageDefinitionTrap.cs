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
        private bool snapToGround = true;

        [SerializeField]
        private bool alignToGround = true;

        [SerializeField]
        private bool faceInDirection;

        public LayerMask GroundLayers => LayerMasks.Ground | LayerMasks.Environment | LayerMasks.Default;

        public bool AlignToGround => alignToGround;

        public bool SnapToGround => snapToGround;

        public TrapDefinition SpawnDefinition => spawn;

        public bool FaceInDirection => faceInDirection;
    }
}
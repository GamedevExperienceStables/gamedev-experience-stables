using NaughtyAttributes;
using UnityEngine;

namespace Game.Level
{
    [CreateAssetMenu(menuName = "Trap/Dynamic Object")]
    public class TrapDynamicDefinition : ScriptableObject
    {
        [SerializeField, Required]
        private TrapView prefab;

        [Space]
        [SerializeField]
        private bool snapToGround = true;

        [SerializeField, ShowIf(nameof(snapToGround))]
        private bool alignToGround;

        [SerializeField]
        private bool randomizeRotationY = true;

        [Space]
        [SerializeField, Expandable]
        private TrapDefinition definition;

        public TrapView Prefab => prefab;
        public TrapDefinition Definition => definition;
        public bool RandomizeRotationY => randomizeRotationY;
        public bool SnapToGround => snapToGround;
        public bool AlignToGround => alignToGround;
    }
}
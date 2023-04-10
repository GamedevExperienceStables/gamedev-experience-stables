using UnityEngine;

namespace Game.Animations.Hero
{
    public static class AnimationNames
    {
        public static readonly int RangeAttack = Animator.StringToHash( "IsRangeAttack");
        public static readonly int MeleeAttack = Animator.StringToHash("IsMeleeAttack");
        public static readonly int XCoordinate = Animator.StringToHash("XCoord");
        public static readonly int YCoordinate = Animator.StringToHash("YCoord");
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int Aiming = Animator.StringToHash("IsAiming");
        public static readonly int Death = Animator.StringToHash("IsDied");
        public static readonly int Damage = Animator.StringToHash("IsDamaged");
        public static readonly int Dash = Animator.StringToHash("IsDash");
        public static readonly int Revive = Animator.StringToHash("IsRevived");
    }
}